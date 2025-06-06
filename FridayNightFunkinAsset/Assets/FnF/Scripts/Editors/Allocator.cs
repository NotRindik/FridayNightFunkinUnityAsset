using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace std
{
         public static unsafe class Allocator
        {
            public static HashSet<IntPtr> pointers { get; private set; } = new HashSet<IntPtr>();
            public static T* Alloc<T>(T value) where T : unmanaged
            {
                T* ptr = (T*)Marshal.AllocHGlobal(sizeof(T));
                if (ptr == null) throw new OutOfMemoryException("Failed to allocate memory.");
                *ptr = value;
                pointers.Add((IntPtr)ptr);
                return ptr;
            }
            public static T* Alloc<T>() where T : unmanaged
            {
                T* ptr = (T*)Marshal.AllocHGlobal(sizeof(T));
                if (ptr == null) throw new OutOfMemoryException("Failed to allocate memory.");
                pointers.Add((IntPtr)ptr);
                return ptr;
            }
            
            public static void Free(void* ptr)
            {
                if (ptr == null) throw new OutOfMemoryException($"null pointer exception with {nameof(ptr)}");
                IntPtr unicTypePointer = (IntPtr)ptr;
                if (!pointers.Remove(unicTypePointer))
                    throw new InvalidOperationException("Attempted to free unmanaged memory not owned by this allocator.");

                Marshal.FreeHGlobal(unicTypePointer);
            }
            
            public static T* AllocArray<T>(int count) where T : unmanaged
            {
                T* ptr = (T*)Marshal.AllocHGlobal(sizeof(T) * count);
                if (ptr == null) throw new OutOfMemoryException("Failed to allocate memory.");
                pointers.Add((IntPtr)ptr);
                return ptr;
            }

            public static void CleanAll()
            {
                foreach (var pointer in pointers)
                {
                    Marshal.FreeHGlobal(pointer);
                }
                pointers.Clear();
            }
        }
         
        public unsafe sealed class UniquePtr<T> : IDisposable where T : unmanaged
        {
            private T* _ptr;
            private bool _disposed;

            public UniquePtr()
            {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    throw new InvalidOperationException("T cannot contain references.");
                _ptr = (T*)Marshal.AllocHGlobal(sizeof(T));
                *_ptr = default;
            }


            public UniquePtr(T value) : this() => *_ptr = value;

            public T* Ptr => _disposed ? throw new ObjectDisposedException(nameof(UniquePtr<T>)) : _ptr;

            public ref T Value => ref *_ptr;

            public void Dispose()
            {
                if (!_disposed)
                {
                    Marshal.FreeHGlobal((IntPtr)_ptr);
                    _ptr = null;
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
            
            public UniquePtr<T> Move()
            {
                var moved = new UniquePtr<T>();
                moved._ptr = _ptr;
                _ptr = null;
                _disposed = true;
                return moved;
            }

            ~UniquePtr() => Dispose();
            
            private UniquePtr(UniquePtr<T> other) { }
            public UniquePtr<T> Clone() => new UniquePtr<T>(*_ptr);
        }
        
        public unsafe sealed class SharedPtr<T> : IDisposable where T : unmanaged
        {
            public sealed class ControlBlock
            {
                public T* Ptr;
                public int StrongRefCount;
                public int WeakRefCount;
            }

            private ControlBlock _control;

            internal  SharedPtr(ControlBlock control)
            {
                _control = control;
            }
            public ControlBlock GetControlBlock() => _control;

            public SharedPtr()
            {
                _control = new ControlBlock
                {
                    Ptr = (T*)Marshal.AllocHGlobal(sizeof(T)),
                    StrongRefCount = 1,
                    WeakRefCount = 0
                };
                *_control.Ptr = default;
            }


            public SharedPtr(T value)
            {
                _control = new ControlBlock
                {
                    Ptr = (T*)Marshal.AllocHGlobal(sizeof(T)),
                    StrongRefCount = 1,WeakRefCount = 0
                };
                *_control.Ptr = value;
            }
            
            public SharedPtr(SharedPtr<T> other)
            {
                _control = other._control;
                Interlocked.Increment(ref _control.StrongRefCount);
            }

            public T* Ptr => _control.Ptr == null ? throw new ObjectDisposedException(nameof(SharedPtr<T>)) : _control.Ptr;
            public ref T Value => ref *_control.Ptr;

            public int UseCount => _control?.StrongRefCount ?? 0;

            public void Dispose()
            {
                if (_control != null)
                {
                    if (Interlocked.Decrement(ref _control.StrongRefCount) == 0)
                    {
                        Marshal.FreeHGlobal((IntPtr)_control.Ptr);
                        _control.Ptr = null;
                    }
                    _control = null;
                }
            }

            ~SharedPtr()
            {
                Dispose();
            }
        }

        public unsafe sealed class WeakPtr<T> : IDisposable where T : unmanaged
        {
            private SharedPtr<T>.ControlBlock _control;


            public WeakPtr(SharedPtr<T> shared)
            {
                if (shared == null) throw new ArgumentNullException(nameof(shared));
                _control = shared.GetControlBlock();
                Interlocked.Increment(ref _control.WeakRefCount);
            }

            public SharedPtr<T>? Lock()
            {
                while (true)
                {
                    int current = _control.StrongRefCount;
                    if (current == 0) return null;

                    if (Interlocked.CompareExchange(ref _control.StrongRefCount, current + 1, current) == current)
                        return new SharedPtr<T>(_control);
                }
            }

            private static SharedPtr<T>.ControlBlock GetControl(SharedPtr<T> shared)
            {
                var field = typeof(SharedPtr<T>).GetField("_control", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (SharedPtr<T>.ControlBlock)field.GetValue(shared);
            }

            ~WeakPtr()
            {
                if (_control != null)
                {
                    if (Interlocked.Decrement(ref _control.WeakRefCount) == 0 && _control.StrongRefCount == 0)
                    {
                        Marshal.FreeHGlobal((IntPtr)_control.Ptr);
                        _control.Ptr = null;
                    }
                }
            }
            public void Reset()
            {
                Dispose();
                _control = null;
            }
            public void Dispose()
            {
                if (_control != null)
                {
                    if (Interlocked.Decrement(ref _control.WeakRefCount) == 0 && _control.StrongRefCount == 0)
                    {
                        Marshal.FreeHGlobal((IntPtr)_control.Ptr);
                        _control.Ptr = null;
                    }
                    _control = null;
                }
                GC.SuppressFinalize(this);
            }

        }

   
}
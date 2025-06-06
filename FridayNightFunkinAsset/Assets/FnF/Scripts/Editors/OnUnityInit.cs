using UnityEditor;
using UnityEngine;

namespace FnF.Scripts.Editors
{
    [InitializeOnLoad]
    public static class OnUnityInit
    {
        static OnUnityInit()
        {
            EditorApplication.quitting += Quit;
        }
        static void Quit()
        {
            std.Allocator.CleanAll();
        }
    }
}
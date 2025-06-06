using UnityEditor;

[InitializeOnLoad]
public static class DomainReloadWatcher
{
    private static Observable<int> a ;
    static DomainReloadWatcher()
    {
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeReload;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterReload;
    }

    private static void OnBeforeReload()
    {
    }

    private static void OnAfterReload()
    {
    }
}
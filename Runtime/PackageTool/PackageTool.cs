using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class PackageTool
{
    [MenuItem("Package/Update KGCoreSystem Package")]
    static void UpdatePackage()
    {
		AssetDatabase.ExportPackage("Assets/KGCoreSystem", "KGCoreSystem.unitypackage", ExportPackageOptions.Recurse);
    }
}
#endif

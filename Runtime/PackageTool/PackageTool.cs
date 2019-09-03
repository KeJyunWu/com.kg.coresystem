using UnityEngine;
using UnityEditor;

public class PackageTool
{
    [MenuItem("Package/Update KGCoreSystem Package")]
    static void UpdatePackage()
    {
		AssetDatabase.ExportPackage("Assets/KGCoreSystem", "KGCoreSystem.unitypackage", ExportPackageOptions.Recurse);
    }
}

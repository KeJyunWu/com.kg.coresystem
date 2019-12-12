using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneMenuAttribute))]
public class SceneMenuDrawer : PropertyDrawer
{

    private const string SCENE_EXTENSION = ".unity";
    private const string NOSCENE_TIP = "Scene is Empty";

    public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
    {
        string sceneFile;
        SceneMenuAttribute attribute = (SceneMenuAttribute)base.attribute;
        List<string> sceneNames = new List<string>();

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {

                sceneFile = EditorBuildSettings.scenes[i].path.Substring(EditorBuildSettings.scenes[i].path.LastIndexOf("/") + 1);
                sceneNames.Add(sceneFile.Replace(SCENE_EXTENSION, string.Empty));
            }
        }

        if (sceneNames.Count == 0)
        {
            EditorGUI.LabelField(position, label.text, NOSCENE_TIP);

        }
        else
        {
            for (int i = 0; i < sceneNames.Count; i++)
            {

                if (sceneNames[i] == property.stringValue)
                {
                    attribute.selected = i;
                    break;
                }
            }
            attribute.selected = EditorGUI.Popup(position, label.text, attribute.selected, sceneNames.ToArray());
            property.stringValue = sceneNames[attribute.selected];
        }
    }
}
#endif
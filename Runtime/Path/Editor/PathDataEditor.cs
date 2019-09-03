using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

[CustomEditor(typeof(PathData))]
public class PathDataEditor : Editor
{

    SerializedProperty m_drawPath;
    SerializedProperty m_crossColor;
    SerializedProperty m_crossLength;
    SerializedProperty m_pathColor;
    SerializedProperty m_pathPoint;

    private void OnEnable()
    {
        m_drawPath = serializedObject.FindProperty("m_drawPath");
        m_crossColor = serializedObject.FindProperty("m_crossColor");
        m_crossLength = serializedObject.FindProperty("m_crossLength");
        m_pathColor = serializedObject.FindProperty("m_pathColor");
        m_pathPoint = serializedObject.FindProperty("m_pathPoint");
    }

    void ChangeSelectedPointIndex(GameObject _object)
    {
        GameObject[] _temp = new GameObject[1] { _object };
        Selection.objects = _temp;
    }

    void GotoScenePoint(Vector3 position)
    {
        Object[] intialFocus = Selection.objects;
        GameObject tempFocusView = new GameObject("Temp Focus View");
        tempFocusView.transform.position = position;
        try
        {
            Selection.objects = new Object[] { tempFocusView };
            SceneView.lastActiveSceneView.FrameSelected();
            Selection.objects = intialFocus;
        }
        catch (NullReferenceException)
        {
            //do nothing
        }
        Object.DestroyImmediate(tempFocusView);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PathData _target = (PathData)target;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Path Data : v0.1");
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(m_drawPath, new GUIContent("Draw ? "));
        EditorGUILayout.PropertyField(m_pathColor, new GUIContent("Path Color"));
        EditorGUILayout.PropertyField(m_crossColor, new GUIContent("Corss Color"));
        EditorGUILayout.PropertyField(m_crossLength, new GUIContent("Corss Length"));

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("List Point Count : " + _target.m_pathPoint.Count.ToString());
        if (GUILayout.Button("Add New Point"))
        {
            _target.AddNewPoint();
            ChangeSelectedPointIndex(_target.m_pathPoint[_target.m_pathPoint.Count-1]);
        }
        EditorGUILayout.EndHorizontal();

        for (var i = 0; i < _target.m_pathPoint.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Point : "+i.ToString(),GUILayout.Width(70));
            if (GUILayout.Button("Select Point"))
            {
                ChangeSelectedPointIndex(_target.m_pathPoint[i]);
            }
            if (GUILayout.Button("GoTo Point"))
            {
                GotoScenePoint(_target.m_pathPoint[i].transform.position);
                ChangeSelectedPointIndex(_target.m_pathPoint[i]);
            }
            if (GUILayout.Button("Remove Point"))
                _target.RemovePoint(i);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.HelpBox("At least two point !", MessageType.Info);

        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}

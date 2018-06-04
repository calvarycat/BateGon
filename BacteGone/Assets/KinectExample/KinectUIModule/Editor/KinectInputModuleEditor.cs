using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using Windows.Kinect;
using System.Collections.Generic;

[CustomEditor(typeof(KinectInputModule))]
public class KinectInputModuleEditor : Editor
{
    private ReorderableList _list;
    private KinectInputModule _kModule;

    private SerializedProperty _allowUpdate;
    private SerializedProperty _targetCanvas;
    private SerializedProperty _scrollTreshold;
    private SerializedProperty _scrollSpeed;
    private SerializedProperty _waitOverTime;

    private void OnEnable()
    {
        _kModule = target as KinectInputModule;
        _list = new ReorderableList(serializedObject, serializedObject.FindProperty("InputData"),
            true, true, true, true);
        _list.drawHeaderCallback += OnDrawHeader;
        _list.drawElementCallback += OnDrawElements;
        _list.onAddDropdownCallback += OnAddDropDown;

        _allowUpdate = serializedObject.FindProperty("AllowUpdate");
        _targetCanvas = serializedObject.FindProperty("TargetCanvas");
        _scrollSpeed = serializedObject.FindProperty("_scrollSpeed");
        _scrollTreshold = serializedObject.FindProperty("_scrollTreshold");
        _waitOverTime = serializedObject.FindProperty("_waitOverTime");
    }

    private void OnAddDropDown(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        if (_kModule.InputData.Length >= 2)
            return;

        if (_kModule.InputData.Length == 0)
        {
            menu.AddItem(new GUIContent("Right Hand"),
                false, OnClickHandler,
                new DataParams {JointType = KinectUIHandType.Right});

            menu.AddItem(new GUIContent("Left Hand"),
                false, OnClickHandler,
                new DataParams {JointType = KinectUIHandType.Left});
        }
        else if (_kModule.InputData.Length == 1)
        {
            DataParams param;
            string handName;

            if (_kModule.InputData[0].HandType == KinectUIHandType.Left)
            {
                param = new DataParams {JointType = KinectUIHandType.Right};
                handName = "Right Hand";
            }
            else
            {
                param = new DataParams {JointType = KinectUIHandType.Left};
                handName = "Left Hand";
            }
            menu.AddItem(new GUIContent(handName), false, OnClickHandler, param);
        }

        menu.ShowAsContext();
    }

    private void OnClickHandler(object dataParams)
    {
        var data = (DataParams)dataParams;
        var index = _list.serializedProperty.arraySize;
        _list.serializedProperty.arraySize++;
        _list.index = index;
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("HandType").enumValueIndex = (int)data.JointType;
        serializedObject.ApplyModifiedProperties();
    }

    private void OnDrawElements(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 3;
        float w = 140f;

        KinectUIHandType ty = (KinectUIHandType)element.FindPropertyRelative("HandType").enumValueIndex;

        EditorGUI.LabelField(new Rect(rect.x, rect.y, w, EditorGUIUtility.singleLineHeight),
            "Tracking Hand: " + ty, EditorStyles.boldLabel);
    }

    private void OnDrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Tracking Hands");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        serializedObject.Update();
        _list.DoLayoutList();

        // Draw other properties
        EditorGUILayout.PropertyField(_allowUpdate, new GUIContent("Allow Update"));
        EditorGUILayout.PropertyField(_targetCanvas, new GUIContent("Target Canvas"));
        EditorGUILayout.PropertyField(_scrollSpeed, new GUIContent("Scroll Speed"));
        EditorGUILayout.PropertyField(_scrollTreshold, new GUIContent("Scroll Treshold"));
        EditorGUILayout.PropertyField(_waitOverTime, new GUIContent("Wait Over Time"));

        serializedObject.ApplyModifiedProperties();
    }

    private struct DataParams
    {
        public KinectUIHandType JointType;
    }
}
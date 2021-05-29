using UnityEditor;
using UnityEngine;

namespace DartCore.UI
{
    [CustomEditor(typeof(Switch))]
    public class SwitchEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("isOn"), new GUIContent("Is On"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isInteractive"),
                new GUIContent("Is Interactive"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnToggle"), new GUIContent("On Toggle"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("normalColor"),
                new GUIContent("Normal Stroke Color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("highlightedColor"),
                new GUIContent("Highlighted Stroke Color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("disabledColor"),
                new GUIContent("Disabled Stroke Color"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bgColorOn"), new GUIContent("BG Color On"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bgColorOff"), new GUIContent("BG Color Off"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fillTransitionSpeed"),
                new GUIContent("Transition Speed"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("toolTip"), new GUIContent("Tooltip Text"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("localizeTooltip"),
                new GUIContent("Localize Tooltip Text"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tooltipTextColor"),
                new GUIContent("Tooltip Text Color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tooltipBgColor"),
                new GUIContent("Tooltip BG Color"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("highlightedClip"),
                new GUIContent("Highlighted Clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pressedClip"), new GUIContent("Pressed Clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mixerGroup"), new GUIContent("Mixer Group"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("volume"), new GUIContent("Volume"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
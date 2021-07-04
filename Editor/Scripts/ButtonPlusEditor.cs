using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace DartCore.UI
{
    [CustomEditor(typeof(ButtonPlus))]
    public class ButtonPlusEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onLeftClick"), new GUIContent("On Left Click"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onRightClick"), new GUIContent("On Right Click"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onMiddleClick"), new GUIContent("On Middle Click"));
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("highlightedClip"), new GUIContent("Highlighted Clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pressedClip"), new GUIContent("Pressed Clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("volume"), new GUIContent("Volume"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mixerGroup"), new GUIContent("Audio Mixer Group"));
            
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playClipOnLeftClick"), new GUIContent("Play Clip on LMB"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playClipOnRightClick"), new GUIContent("Play Clip on RMB"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playClipOnMiddleClick"), new GUIContent("Play Clip on MMB"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
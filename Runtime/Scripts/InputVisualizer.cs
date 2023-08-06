using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DartCore.Utilities;
# if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(Image))]
    public class InputVisualizer : MonoBehaviour
    {
        #region Unity Editor
#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Input Visualizer", priority=22), MenuItem("GameObject/UI/DartCore/Input Visualizer", priority=22)]
        public static void AddInputVisualizer()
        {
            if (Selection.activeGameObject == null && FindObjectOfType<Canvas>() == null)
            {
                var canvas = new GameObject { name = "New Canvas" };

                var canvasComp = canvas.AddComponent<Canvas>();
                canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;

                canvas.AddComponent<CanvasScaler>();
                canvas.AddComponent<GraphicRaycaster>();
            }

            var objParent = Selection.activeGameObject != null ? Selection.activeGameObject.transform :
                FindObjectOfType<Canvas>() != null ? FindObjectOfType<Canvas>().transform : null;
            var obj = Instantiate(Resources.Load<GameObject>("InputVisualizer"),
                objParent, false);
            obj.name = "New Input Visualizer";
        }
#endif
        #endregion

        [Header("Behaviours")]
        public bool autoPickGamepad;
        public GamepadPlatform currentGamepadPlatform = GamepadPlatform.Xbox;
        
        [Header("Styles")]
        public InputVisualizerStyle playstationStyle;
        public InputVisualizerStyle xboxStyle;
        public InputVisualizerStyle nintendoStyle;
        public Sprite keyboardSprite;
        [Space]
        public GamepadKey key;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (autoPickGamepad)
                AutoPickGamepad();
        }

        public void UpdateGamepad(GamepadPlatform desiredPlatform)
        {
            if (currentGamepadPlatform == desiredPlatform) return;
            
            currentGamepadPlatform = desiredPlatform;
            UpdateImage();
        }

        private void UpdateImage()
        {
            var platform = InputUtilities.GetGamepadPlatform();
            var styleToUse = platform switch
            {
                GamepadPlatform.Playstation => playstationStyle,
                GamepadPlatform.Nintendo => nintendoStyle,
                GamepadPlatform.Xbox => xboxStyle,
                _ => null
            };
            
            image.sprite = styleToUse ? styleToUse.GetSpriteOfKey(key) : keyboardSprite;
        }

        private void AutoPickGamepad() => UpdateGamepad(InputUtilities.GetGamepadPlatform());
    }
}

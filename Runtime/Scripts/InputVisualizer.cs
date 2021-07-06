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
        [MenuItem("DartCore/UI/Input Visualizer"), MenuItem("GameObject/UI/DartCore/Input Visualizer")]
        public static void AddButtonPlus()
        {
            if (Selection.activeGameObject == null && FindObjectOfType<Canvas>() == null)
            {
                var canvas = new GameObject();
                canvas.name = "New Canvas";
                
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

        public bool autoPickController = false;
        public bool disableOnKeyboard = false;
        public ControllerType currentController = ControllerType.XBoxOne;
        [Space]
        
        [Tooltip("This style will be used for the controllers Dualshock3 & Dualshock 4")]
        public InputVisualizerStyle dualshockStyle;
        
        [Tooltip("This style will be used for the controllers other than Dualshock3 & Dualshock 4")]
        public InputVisualizerStyle xboxStyle;
        [Space]
        public ControllerKey key;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (autoPickController)
                AutoPickController();

            UpdateImage();
        }

        public void UpdateController(ControllerType desiredController)
        {
            currentController = desiredController;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (disableOnKeyboard && currentController == ControllerType.None)
            {
                image.enabled = false;
                return;
            }

            var styleToUse = InputUtilities.IsDualshock(currentController) ? dualshockStyle : xboxStyle;
            
            if (!styleToUse) return;
            image.sprite = styleToUse.GetSpriteOfKey(key);
            
            image.enabled = true;
        }

        private void AutoPickController()
        {
            currentController = InputUtilities.GetMainController();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [ExecuteInEditMode, HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#4-progress-bar")]
    public class ProgressBar : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Linear Progress Bar"), MenuItem("GameObject/UI/DartCore/Linear Progress Bar")]
        public static void AddLinearProgressBar()
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
            var obj = Instantiate(Resources.Load<GameObject>("Linear Progress Bar"),
                objParent, false);
            obj.name = "New Linear Progress Bar";
        }

        [MenuItem("DartCore/UI/Radial Progress Bar"), MenuItem("GameObject/UI/DartCore/Radial Progress Bar")]
        public static void AddRadialProgressBar()
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
            var obj = Instantiate(Resources.Load<GameObject>("Radial Progress Bar"),
                objParent, false);
            obj.name = "New Radial Progress Bar";
        }
#endif

        #endregion

        [Tooltip("true: limits the current between min & max")]
        public bool hasBoundries = true;

        public float min;
        public float max = 1;
        public float current = 0;
        [FormerlySerializedAs("fillTime")] [Range(1f, 20f)] public float fillSpeed = 8f;
        [Tooltip("If set to true, fillSpeed will be ignored.")] public bool instantFill = false;
        public Color bgColor = Color.white;
        public Color fillerColor = Color.red;
        public bool isRadial;
        [Range(0, 1)] public float innerRadius;
        public Sprite innerCircleIcon;
        public Color innerCircleIconColor;
        public Color innerCircleColor;

        private Image mask;
        private Image filler;
        private Image innerCircle;
        private RectTransform innerCircleRect;
        private Image innerCircleImage;
        private Image outerCircleMask;
        private Image bgImage;
        [Range(0f, 10f)] public float outerCircleRadius = 5f;

        private void Awake()
        {
            if (isRadial)
            {
                outerCircleMask = transform.Find("Masking").GetComponent<Image>();
                bgImage = outerCircleMask.transform.Find("BG").GetComponent<Image>();
                mask = outerCircleMask.transform.Find("Mask").GetComponent<Image>();
                filler = mask.transform.Find("Fill").GetComponent<Image>();
                innerCircle = transform.Find("InnerCircle").GetComponent<Image>();
                innerCircleImage = innerCircle.transform.Find("Icon").GetComponent<Image>();
                innerCircleRect = innerCircle.GetComponent<RectTransform>();
            }
            else
            {
                bgImage = GetComponent<Image>();
                mask = transform.Find("Mask").GetComponent<Image>();
                filler = mask.transform.Find("Fill").GetComponent<Image>();
            }
        }

        private void Update()
        {
            #region UnityEditor

#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                bgImage.color = bgColor;
                filler.color = fillerColor;
                mask.fillAmount = (current - min) / (max - min);
            }
#endif

            #endregion

            if (hasBoundries)
                current = Mathf.Clamp(current, min, max);

            bgImage.color = Color.Lerp(bgImage.color, bgColor, fillSpeed * Time.unscaledDeltaTime);
            GetCurrentFill();
            if (isRadial)
            {
                UpdateInnerCircle();
                outerCircleMask.pixelsPerUnitMultiplier = Mathf.Lerp(outerCircleMask.pixelsPerUnitMultiplier,
                    outerCircleRadius, fillSpeed * Time.unscaledDeltaTime);
                outerCircleMask.SetAllDirty();
            }
        }

        private void GetCurrentFill()
        {
            var currentOffset = current - min;
            var maxOffset = max - min;

            var desiredFillAmount = currentOffset / maxOffset;
            if (mask)
            {
                if (!instantFill)   
                    mask.fillAmount = Mathf.Lerp(mask.fillAmount, desiredFillAmount, fillSpeed * Time.unscaledDeltaTime);
                else
                    mask.fillAmount = desiredFillAmount;
            }

            filler.color = Color.Lerp(filler.color, fillerColor, fillSpeed * Time.unscaledDeltaTime);
        }

        private void UpdateInnerCircle()
        {
            innerCircleRect.localScale = Vector2.one * innerRadius;
            innerCircleImage.sprite = innerCircleIcon;
            innerCircleImage.color = innerCircleIconColor;
            innerCircle.color = innerCircleColor;
        }

        public float GetPercentage(int digits = 2)
        {
            var percentage = current * 100 / (max - min);
            return (float) Math.Round((decimal) percentage, digits);
        }
    }
}
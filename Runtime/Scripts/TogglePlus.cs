using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [ExecuteInEditMode, HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#2-toggleplus")]
    public class TogglePlus : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Toggle Plus"), MenuItem("GameObject/UI/DartCore/Toggle Plus")]
        public static void AddTogglePlus()
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
            var obj = Instantiate(Resources.Load<GameObject>("TogglePlus"),
                objParent, false);
            obj.name = "New Toggle Plus";
        }
#endif

        #endregion

        public bool isOn = false;
        public bool isInteractive = true;
        protected bool wasInteractive = true;

        [SerializeField] private UnityEvent OnToggle;

        [Header("Colors")] public Color normalColor;
        public Color highlightedColor;
        public Color disabledColor;
        [FormerlySerializedAs("transitionDuration")]
        [Range(1f, 20f)] public float colorTransitionSpeed = 8f;

        [FormerlySerializedAs("fillTransitionDuration"),
        Header("Filling"), Range(1f, 20f)] public float fillTransitionSpeed = 8f;
        private float currentFillTransitionSpeed;
        public Color fillColor = Color.red;
        public ToggleFillAnimation animType;
        [Range(0, 1)] public float fillScale = .8f;

        [Header("Tooltip")] public string toolTip;

        [Tooltip("toolTip will be used as a key if set to true")]
        public bool localizeTooltip = false;

        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Header("Audio")] public AudioClip highlightedClip;
        public AudioClip pressedClip;
        public AudioMixerGroup mixerGroup;
        [Range(0, 1)] public float volume = .2f;

        private Image mask;
        private Image fill;

        private RectTransform maskRect;
        private Image backgroundImage;
        private Color backgroundColor;

        private void Awake()
        {
            mask = transform.Find("Mask").GetComponent<Image>();
            maskRect = mask.GetComponent<RectTransform>();
            backgroundImage = GetComponent<Image>();

            fill = mask.transform.Find("Fill").GetComponent<Image>();
            NormalState();
        }

        private void Update()
        {
            #region UnityEditor

#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                fill.color = fillColor;
            }
#endif

            #endregion

            UpdateFill();
            if (!isInteractive)
                DisabledState();
            if (isInteractive && !wasInteractive)
                NormalState();

            if (isOn)
                fill.color = Color.Lerp(fill.color, fillColor,(animType == ToggleFillAnimation.Fade ?
                    currentFillTransitionSpeed : colorTransitionSpeed) * Time.unscaledDeltaTime);
 
            wasInteractive = isInteractive;
            if (backgroundImage)
                backgroundImage.color = Color.Lerp(backgroundImage.color, backgroundColor,
                    colorTransitionSpeed * Time.unscaledDeltaTime);
        }

        private void UpdateFill()
        {
            fillScale = Mathf.Clamp(fillScale, 0f, 1f);

            if (maskRect) maskRect.localScale = Vector2.one * fillScale;
            switch (animType)
            {
                case ToggleFillAnimation.Horizontal:
                    currentFillTransitionSpeed = fillTransitionSpeed;
                    mask.fillMethod = Image.FillMethod.Horizontal;
                    break;
                case ToggleFillAnimation.Vertical:
                    currentFillTransitionSpeed = fillTransitionSpeed;
                    mask.fillMethod = Image.FillMethod.Vertical;
                    break;
                case ToggleFillAnimation.Radial90:
                    currentFillTransitionSpeed = fillTransitionSpeed;
                    mask.fillMethod = Image.FillMethod.Radial90;
                    break;
                case ToggleFillAnimation.Radial180:
                    currentFillTransitionSpeed = fillTransitionSpeed;
                    mask.fillMethod = Image.FillMethod.Radial180;
                    break;
                case ToggleFillAnimation.Radial360:
                    currentFillTransitionSpeed = fillTransitionSpeed;
                    mask.fillMethod = Image.FillMethod.Radial360;
                    break;
                case ToggleFillAnimation.Fade:
                    currentFillTransitionSpeed = fillTransitionSpeed;
                    mask.fillAmount = 1;
                    if (!isOn)
                        fill.color = Color.Lerp(fill.color, Color.clear, currentFillTransitionSpeed * Time.unscaledDeltaTime);
                    break;
                case ToggleFillAnimation.None:
                    currentFillTransitionSpeed = 0f;
                    break;
                default:
                    break;
            }

            if (animType != ToggleFillAnimation.Fade)
                mask.fillAmount = Mathf.Lerp(mask.fillAmount, isOn ? 1 : 0, currentFillTransitionSpeed * Time.unscaledDeltaTime);
        }

        private void Click()
        {
            if (!isInteractive) return;
            
            isOn = !isOn;
            OnToggle.Invoke();
            UIAudioManager.PlayOneShotAudio(pressedClip, volume, mixerGroup);
        }

        private void Highlight()
        {
            if (!isInteractive) return;

            backgroundColor = highlightedColor;
            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeTooltip);
            UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
        }

        protected void NormalState()
        {
            if (!isInteractive) return;

            backgroundColor = normalColor;
            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }

        protected void DisabledState()
        {
            if (!isInteractive)
                backgroundColor = disabledColor;
        }

        #region Cursor Detection

        public void OnPointerClick(PointerEventData eventData) => Click();
        public void OnPointerEnter(PointerEventData eventData) => Highlight();
        public void OnPointerExit(PointerEventData eventData) => NormalState();

        #endregion
    }

    public enum ToggleFillAnimation : byte
    {
        Horizontal = 0,
        Vertical = 1,
        Radial90 = 2,
        Radial180 = 3,
        Radial360 = 4,
        Fade = 5,
        None = 6,
    }
}
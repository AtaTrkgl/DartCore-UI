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
    public class TogglePlus : Selectable, ISubmitHandler
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
        protected bool wasInteractive = true;

        [FormerlySerializedAs("OnToggle")] [SerializeField] private UnityEvent onToggle;

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

        private void Awake()
        {
            mask = transform.Find("Mask").GetComponent<Image>();
            maskRect = mask.GetComponent<RectTransform>();

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
            if (interactable && !wasInteractive)
                NormalState();

            if (isOn)
                fill.color = Color.Lerp(fill.color, fillColor,currentFillTransitionSpeed * Time.unscaledDeltaTime);
 
            wasInteractive = interactable;
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
            if (!interactable) return;
            
            isOn = !isOn;
            onToggle.Invoke();
            UIAudioManager.PlayOneShotAudio(pressedClip, volume, mixerGroup);
        }

        private void Highlight()
        {
            if (!interactable) return;

            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeTooltip);
            UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
        }

        protected void NormalState()
        {
            if (!interactable) return;

            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }

        #region Cursor Detection

        public void OnSubmit(BaseEventData eventData)
        {
            Click();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            Click();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Highlight();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            NormalState();
        }

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
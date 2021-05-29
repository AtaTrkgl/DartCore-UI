using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#1-buttonplus")]
    public class ButtonPlus : Button, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/ButtonPlus"), MenuItem("GameObject/UI/DartCore/ButtonPlus")]
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
            var obj = Instantiate(Resources.Load<GameObject>("ButtonPlus"), objParent,
                false);
            obj.name = "New Button Plus";
        }
#endif

        #endregion

        public UnityEvent onLeftClick;
        public UnityEvent onRightClick;
        public UnityEvent onMiddleClick;
        
        [Header("Tooltip")] public string toolTip;
        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Tooltip("toolTip will be used as a key if set to true")]
        public bool localizeText = false;

        [Header("Audio")] public AudioClip highlightedClip;
        public AudioClip pressedClip;
        public AudioMixerGroup mixerGroup;
        [Range(0, 1)] public float volume = .2f;
        public bool playClipOnRightClick = true;
        public bool playClipOnLeftClick = true;
        public bool playClipOnMiddleClick = true;

        private void ClickSound()
        {
            if (base.interactable)
            {
                UIAudioManager.PlayOneShotAudio(pressedClip, volume, mixerGroup);
            }
        }

        private void Exit()
        {
            if (!base.interactable) return;

            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }

        private void Highlight()
        {
            if (!base.interactable) return;

            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeText);
            UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
        }

        #region Cursor Detection

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Highlight();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Exit();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    onLeftClick?.Invoke();
                    if (playClipOnLeftClick) ClickSound();
                    break;
                case PointerEventData.InputButton.Right:
                    onRightClick?.Invoke();
                    if (playClipOnRightClick) ClickSound();
                    break;
                case PointerEventData.InputButton.Middle:
                    onMiddleClick?.Invoke();
                    if (playClipOnMiddleClick) ClickSound();
                    break;
            }
            
            base.OnPointerClick(eventData);
        }

        #endregion
    }
}
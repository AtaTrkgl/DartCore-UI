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
    public class ButtonPlus : Button
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/ButtonPlus", priority=11), MenuItem("GameObject/UI/DartCore/ButtonPlus", priority=11)]
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

        private void Highlight()
        {
            if (!base.interactable) return;

            UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
        }

        #region Cursor Detection

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Highlight();
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

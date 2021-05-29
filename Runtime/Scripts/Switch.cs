using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#3-switch")]
    public class Switch : TogglePlus
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Switch"), MenuItem("GameObject/UI/DartCore/Switch")]
        public static void AddSwitch()
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
            var obj = Instantiate(Resources.Load<GameObject>("Switch"),
                objParent, false);
            obj.name = "New Switch";
        }
#endif

        #endregion

        public Color bgColorOn = Color.green;
        public Color bgColorOff = Color.white;

        private RectTransform circle;
        private RectTransform rectTrans;
        private Image bgFill;
        private Image bg;
        private float circleX;
        private const float PADDING = .5f;

        private void Awake()
        {
            base.NormalState();

            rectTrans = GetComponent<RectTransform>();
            circle = transform.Find("Circle").GetComponent<RectTransform>();
            bgFill = transform.Find("BG Fill Mask").Find("BG Fill").GetComponent<Image>();
            bg = transform.Find("BG").GetComponent<Image>();
        }

        private void Update()
        {
            #region UnityEditor

#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                bgFill.fillAmount = isOn ? 1 : 0;
                bg.color = bgColorOn;
                bgFill.color = bgColorOff;
            }
#endif

            #endregion

            if (!isInteractive)
                DisabledState();
            if (isInteractive && !wasInteractive)
                NormalState();

            circleX = rectTrans.sizeDelta.x * .5f - circle.sizeDelta.x * .5f - PADDING;

            circle.localPosition = new Vector3(Mathf.Lerp(circle.localPosition.x, isOn ? circleX : -circleX,
                fillTransitionSpeed * Time.unscaledDeltaTime), circle.localPosition.y, circle.localPosition.z);

            bgFill.fillAmount = Mathf.Lerp(bgFill.fillAmount, isOn ? 1 : 0, fillTransitionSpeed * .7f * Time.unscaledDeltaTime);

            bg.color = Color.Lerp(bg.color, bgColorOn, fillTransitionSpeed * Time.unscaledDeltaTime);
            bgFill.color = Color.Lerp(bgFill.color, bgColorOff, fillTransitionSpeed * Time.unscaledDeltaTime);

            wasInteractive = isInteractive;
        }
    }
}
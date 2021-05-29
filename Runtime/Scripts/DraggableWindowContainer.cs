using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#5-tooltip")]
    public class DraggableWindowContainer : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Draggable Window Container"),
         MenuItem("GameObject/UI/DartCore/Draggable Window Container")]
        public static void CreateDraggableWindowContainer()
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
            var obj = Instantiate(Resources.Load<GameObject>("DraggableWindowContainer"),
                objParent, false);
            obj.name = "New Draggable Window Container";
        }
#endif

        #endregion

        [Header("Configuration")] [SerializeField]
        private float padding;

        [FormerlySerializedAs("followTime"), Range(1f, 20f)] public float followSpeed = 12f;

        private RectTransform containerTrans;
        private RectTransform draggableWindowTrans;
        private RectTransform canvas;
        private DraggableWindow draggableWindow;
        private Vector2 cursorOffset;
        private bool isDragging = false;
        private Vector4 boundaries;

        private void Awake()
        {
            containerTrans = GetComponent<RectTransform>();
            draggableWindowTrans = transform.Find("DraggableWindow").GetComponent<RectTransform>();
            draggableWindow = transform.Find("DraggableWindow").GetComponent<DraggableWindow>();
            canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            UpdatePadding();
        }

        private void Update()
        {
            if (draggableWindow.isCursorOn && Input.GetMouseButtonDown(0))
            {
                cursorOffset = (Vector2) Input.mousePosition / canvas.lossyScale -
                               (Vector2) draggableWindowTrans.localPosition;
                isDragging = true;
            }

            if (isDragging)
            {
                if (Input.GetMouseButton(0))
                    FollowCursor();
                if (Input.GetMouseButtonUp(0))
                    isDragging = false;
            }
        }

        public void SetPadding(float val)
        {
            padding = val;
            UpdatePadding();
        }

        private void FollowCursor()
        {
            var desiredPos = (Vector2) Input.mousePosition / canvas.lossyScale - cursorOffset;
            draggableWindowTrans.localPosition = new Vector2(
                Mathf.Lerp(draggableWindowTrans.localPosition.x, Mathf.Clamp(desiredPos.x, boundaries.x, boundaries.y), followSpeed * Time.unscaledDeltaTime),
                Mathf.Lerp(draggableWindowTrans.localPosition.y, Mathf.Clamp(desiredPos.y, boundaries.z, boundaries.w), followSpeed * Time.unscaledDeltaTime));
        }

        private void UpdatePadding()
        {
            var sizeDelta = containerTrans.sizeDelta;
            var delta = draggableWindowTrans.sizeDelta;

            boundaries = new Vector4(
                -sizeDelta.x / 2 + delta.x / 2 + padding,
                sizeDelta.x / 2 - delta.x / 2 - padding,
                -sizeDelta.y / 2 + delta.y / 2 + padding,
                sizeDelta.y / 2 - delta.y / 2 - padding
            );
        }
    }
}
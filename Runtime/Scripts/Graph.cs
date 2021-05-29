using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
# if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    public enum GraphType : byte
    {
        Line = 0,
        Dot = 1,
        Bar = 2,
    }

    [SelectionBase, ExecuteInEditMode, HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#5-tooltip")]
    public class Graph : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Graph"), MenuItem("GameObject/UI/DartCore/Graph")]
        public static void AddGraph()
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
            var obj = Instantiate(Resources.Load<GameObject>("Graph"), objParent,
                false);
            obj.name = "New Graph";
        }
#endif

        #endregion

        [Header("Marker")] [SerializeField] private Sprite markerSprite;
        [SerializeField] private Sprite barSprite;
        [SerializeField] private float markerScale = 11f;
        [SerializeField] private float markerConnectorScale = 3f;
        [SerializeField] private Color markerColor = Color.white;
        [SerializeField] private Color markerConnectorColor = Color.white;

        [Header("Graph")] [SerializeField] private float graphElementsPadding = 53f;
        [SerializeField] private float graphBGPadding = 25f;

        [Header("Tooltip")] [SerializeField] private bool displayTooltips = true;
        [SerializeField] private Color tooltipBgColor;
        [SerializeField] private Color tooltipTextColor;
        public float maxHeight = 100;

        private RectTransform graphContainer;
        private RectTransform markersParent;
        private RectTransform connectorsParent;

        private void Awake()
        {
            graphContainer = transform.Find("Graphics Container").GetComponent<RectTransform>();
            markersParent = graphContainer.transform.Find("Markers").GetComponent<RectTransform>();
            connectorsParent = graphContainer.transform.Find("Connectors").GetComponent<RectTransform>();

            graphContainer.anchorMin = Vector2.zero;
            graphContainer.anchorMax = Vector2.zero;
        }

        private void Update()
        {
            UpdateGraphicsContainerSize();
        }

        public void ShowGraph(List<int> valueList, GraphType graphType, bool ignoreMaxHeight)
        {
            ClearGraph();

            switch (graphType)
            {
                case GraphType.Line:
                    CreateSimpleGraph(valueList, ignoreMaxHeight, false);
                    break;
                case GraphType.Dot:
                    CreateSimpleGraph(valueList, ignoreMaxHeight, true);
                    break;
                case GraphType.Bar:
                    CreateBarGraph(valueList, ignoreMaxHeight);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This function can handle both line & scatter plots
        /// </summary>
        /// <param name="valueList"> values</param>
        /// <param name="ignoreMaxHeight"> if the variable max height should be ignored or not</param>
        /// <param name="isDotGraph"></param>
        private void CreateSimpleGraph(List<int> valueList, bool ignoreMaxHeight, bool isDotGraph)
        {
            var graphHeight = graphContainer.sizeDelta.y - 2 * markerScale;
            var yMax = maxHeight;

            var greatestIndex = 0;
            for (var i = 0; i < valueList.Count; i++)
            {
                if (valueList[i] > valueList[greatestIndex])
                    greatestIndex = i;
            }

            if (ignoreMaxHeight || valueList[greatestIndex] > maxHeight)
                yMax = valueList[greatestIndex];

            float xSize = (graphContainer.sizeDelta.x - 2 * graphElementsPadding) /
                          (valueList.Count > 1 ? valueList.Count : 2);
            GameObject lastMarker = null;

            for (int i = 0; i < valueList.Count; i++)
            {
                valueList[i] = (int) Mathf.Clamp(valueList[i], 0f, yMax);
                float xPos = (i + .5f) * xSize + graphElementsPadding;
                float yPos = (valueList[i] / yMax) * graphHeight + markerScale;
                var marker = CreateMarker(new Vector2(xPos, yPos), displayTooltips, valueList[i].ToString());
                if (lastMarker && !isDotGraph)
                {
                    var markerRect = marker.GetComponent<RectTransform>();
                    CreateMarkerConnection(markerRect.anchoredPosition, markerRect.anchoredPosition);
                }

                lastMarker = marker;
            }
        }

        private void CreateBarGraph(List<int> valueList, bool ignoreMaxHeight)
        {
            var graphHeight = graphContainer.sizeDelta.y - 2 * markerScale;
            var yMax = maxHeight;

            var greatestIndex = 0;
            for (var i = 0; i < valueList.Count; i++)
            {
                if (valueList[i] > valueList[greatestIndex])
                    greatestIndex = i;
            }

            if (ignoreMaxHeight || valueList[greatestIndex] > maxHeight)
                yMax = valueList[greatestIndex];

            var xSize = (graphContainer.sizeDelta.x - 2 * graphElementsPadding) /
                        (valueList.Count > 1 ? valueList.Count : 2);

            for (int i = 0; i < valueList.Count; i++)
            {
                valueList[i] = (int) Mathf.Clamp(valueList[i], 0f, yMax);
                var xPos = (i + .5f) * xSize + graphElementsPadding;
                var yPos = (valueList[i] / yMax) * graphHeight + markerScale;
                CreateBar(yPos, xPos, xSize * .5f, displayTooltips, valueList[i].ToString());
            }
        }

        //Linear & Scatter Plots
        private void CreateMarkerConnection(Vector2 markerPosA, Vector2 markerPosB)
        {
            GameObject gameObj = new GameObject("markerConnection", typeof(Image));
            gameObj.transform.SetParent(connectorsParent, false);

            gameObj.GetComponent<Image>().color = markerConnectorColor;

            var rectTrans = gameObj.GetComponent<RectTransform>();
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.zero;
            rectTrans.pivot = Vector2.zero;
            rectTrans.sizeDelta = new Vector2(Vector2.Distance(markerPosA, markerPosB), markerConnectorScale);
            rectTrans.anchoredPosition = markerPosA;

            var angle = Mathf.Atan2(markerPosB.y - markerPosA.y, markerPosB.x - markerPosA.x);
            rectTrans.localEulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        }

        private GameObject CreateMarker(Vector2 anchoredPos, bool addToolTip = false, string toolTipText = "")
        {
            var gameObj = new GameObject("marker", typeof(Image));
            gameObj.transform.SetParent(markersParent, false);

            var objImg = gameObj.GetComponent<Image>();
            objImg.sprite = markerSprite;
            objImg.color = markerColor;

            var rectTrans = gameObj.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = anchoredPos;
            rectTrans.sizeDelta = new Vector2(markerScale, markerScale);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.zero;

            if (addToolTip)
            {
                var toolTipTarget = gameObj.AddComponent<TooltipTarget>();
                toolTipTarget.toolTip = toolTipText;
                toolTipTarget.tooltipBgColor = tooltipBgColor;
                toolTipTarget.tooltipTextColor = tooltipTextColor;
            }

            return gameObj;
        }

        //Bar Plots
        private void CreateBar(float height, float xPos, float width, bool addToolTips, string tooltipText = "")
        {
            var gameObj = new GameObject("marker", typeof(Image));
            gameObj.transform.SetParent(markersParent, false);

            var objImg = gameObj.GetComponent<Image>();
            objImg.sprite = barSprite;
            objImg.color = markerColor;

            if (addToolTips)
            {
                var toolTipTarget = gameObj.AddComponent<TooltipTarget>();
                toolTipTarget.toolTip = tooltipText;
                toolTipTarget.tooltipBgColor = tooltipBgColor;
                toolTipTarget.tooltipTextColor = tooltipTextColor;
            }


            var rectTrans = gameObj.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(xPos, 0);
            rectTrans.sizeDelta = new Vector2(markerScale, markerScale);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.zero;
            rectTrans.pivot = new Vector2(.5f, 0f);
            rectTrans.sizeDelta = new Vector2(width, height);
        }

        public void ClearGraph()
        {
            foreach (Transform child in markersParent)
                Destroy(child.gameObject);

            foreach (Transform child in connectorsParent)
                Destroy(child.gameObject);
        }

        private void UpdateGraphicsContainerSize()
        {
            graphContainer.sizeDelta = GetComponent<RectTransform>().sizeDelta - Vector2.one * (2 * graphBGPadding);
            graphContainer.anchoredPosition = Vector2.one * graphBGPadding;
        }
    }
}
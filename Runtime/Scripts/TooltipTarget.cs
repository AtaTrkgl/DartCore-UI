using UnityEngine;
using UnityEngine.EventSystems;

namespace DartCore.UI
{
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Tooltip")] public string toolTip;

        [Tooltip("Leaving this value at 0 will ignore length limit.")]
        public int maxLineLength = 25;
        [Tooltip("toolTip will be used as a key if set to true")]
        public bool localizeToolTip = false;

        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeToolTip, maxLineLength);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }
    }
}
﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DartCore.UI
{
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [Header("Tooltip")] public string toolTip;

        [Tooltip("Leaving this value at 0 will ignore length limit.")]
        public int maxLineLength = 25;
        [Tooltip("toolTip will be used as a key if set to true")]
        public bool localizeToolTip = false;

        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Header("Selection")]
        public bool displayOnSelection = false;
        public RectTransform selectionDisplayPosition;
        
        public void Display(Vector2 positionOverride)
        {
            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor,positionOverride, localizeToolTip, maxLineLength);
        }
        
        public void Display()
        {
            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeToolTip, maxLineLength);
        }

        public void Hide()
        {
            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }

        public void OnPointerEnter(PointerEventData eventData) => Display();

        public void OnPointerExit(PointerEventData eventData) => Hide();

        public void OnSelect(BaseEventData eventData)
        {
            if (!displayOnSelection) return;

            Display(selectionDisplayPosition.position);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (displayOnSelection) Hide();
        }
    }
}
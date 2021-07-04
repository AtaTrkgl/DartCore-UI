using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DartCore.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownAutoScroller : MonoBehaviour
    {
        public bool isActive = true;

        [Tooltip("It is recommended to set this to false when a controller is being used and setting to true when Keyboard and Mouse if being used.")]
        public bool onlyWorkOnActivation = false;

        private bool hasReachedTargetHeight = false;
        
        private EventSystem eventSystem;
        private GameObject lastSelection;
        
        private TMP_Dropdown dropdown;
        private float itemHeight;

        private RectTransform currentContent;

        public bool useUnscaledTime = false;
        [Range(1f, 100f)] public float autoScrollSpeed = 10f;
        private float desiredHeight = 0f;
        
        private void Awake()
        {
            eventSystem = EventSystem.current;
            dropdown = GetComponent<TMP_Dropdown>();
            
            // Accesses the Template Items RectTransform.
            itemHeight = dropdown.template.GetChild(0).GetChild(0).GetChild(0).
                GetComponent<RectTransform>().sizeDelta.y;
        }

        private void Update()
        {
            if (!isActive) return;
            
            if (currentContent && (!onlyWorkOnActivation || !hasReachedTargetHeight))
            {
                var currentHeight = Mathf.Lerp(currentContent.anchoredPosition.y, desiredHeight, 
                    autoScrollSpeed * (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
                
                currentContent.anchoredPosition = new Vector2(currentContent.anchoredPosition.x,currentHeight);
                
                if (Mathf.Abs(currentHeight - desiredHeight) < .5f) hasReachedTargetHeight = true;
            }

            if (lastSelection != gameObject && eventSystem.currentSelectedGameObject == gameObject)
                hasReachedTargetHeight = false;
            
            if (hasReachedTargetHeight && onlyWorkOnActivation) return;
            
            if (lastSelection != eventSystem.currentSelectedGameObject) ScrollToSelectedIndex();
            
            lastSelection = eventSystem.currentSelectedGameObject;
        }

        public void ScrollToSelectedIndex()
        {
            if (!dropdown.IsExpanded) return;
            
            if (!currentContent)
                currentContent = transform.Find("Dropdown List").GetChild(0).GetChild(0).GetComponent<RectTransform>();

            for (var i = 0; i < currentContent.transform.childCount; i++)
            {
                var child = currentContent.transform.GetChild(i);
                if (EventSystem.current.currentSelectedGameObject == child.gameObject)
                {
                    ScrollToIndex(i);
                    return;
                }
            }
        }

        private void ScrollToIndex(int index)
        {
            desiredHeight = itemHeight * (index - 1);
        }
    }
}


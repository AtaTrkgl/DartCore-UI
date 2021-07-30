using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DartCore.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollViewAutoScroller : MonoBehaviour
    {
        [Header("Config")]
        public bool isActive = true;
        public bool updateVerticalNavOnEnable = false;
        
        private bool hasReachedTargetHeight = false;
        
        private EventSystem eventSystem;
        private GameObject lastSelection;
        private RectTransform content;
        private RectTransform rectTransform;

        [Header("Settings")]
        public bool useUnscaledTime = false;
        public float offset = 0f;
        [Range(1f, 100f)] public float autoScrollSpeed = 10f;

        private float desiredHeight = 0f;
        private Selectable[] children;
        
        private void Awake()
        {
            eventSystem = EventSystem.current;
            rectTransform = GetComponent<RectTransform>();
            content = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            UpdateChildren();
            
            if (updateVerticalNavOnEnable)
                UpdateVerticalNav();
        }
        
        private void Update()
        {
            if (!isActive) return;

            // Update the desired height so if the size gets changed the height can adjust.
            SetDesiredHeight(desiredHeight);
            
            content.anchoredPosition = new Vector2(content.anchoredPosition.x,
                Mathf.Lerp(content.anchoredPosition.y, desiredHeight, autoScrollSpeed * Time.deltaTime));
            
            if (lastSelection != eventSystem.currentSelectedGameObject)
                TryScrollToSelectedIndex();
            
            lastSelection = eventSystem.currentSelectedGameObject;
        }
        
        public void UpdateChildren()
        {
            var selectables = new List<Selectable>();
            foreach (Transform child in transform.GetChild(0).GetChild(0))
            {
                var selectable = child.GetComponent<Selectable>();
                if (selectable)
                    selectables.Add(selectable);
            }

            children = selectables.ToArray();
        }

        public void UpdateVerticalNav()
        {
            for (var i = 0; i < children.Length; i++)
            {
                var nav = children[i].navigation;
                nav.mode = Navigation.Mode.Explicit;

                nav.selectOnDown = children[i + 1 == children.Length ? 0 : i + 1];
                nav.selectOnUp = children[i - 1 == -1 ? children.Length - 1 : i - 1];

                children[i].navigation = nav;
            }
        }

        public void TryScrollToSelectedIndex()
        {
            for (var i = 0; i < children.Length; i++)
            {
                if (children[i] == null)
                {
                    UpdateChildren();
                    return;
                }

                if (EventSystem.current.currentSelectedGameObject != children[i].gameObject) continue;
                
                ScrollToIndex(i);
                return;
            }
        }

        private void ScrollToIndex(int index)
        {
            var selectedRect = children[index].GetComponent<RectTransform>();
            SetDesiredHeight(-selectedRect.anchoredPosition.y - selectedRect.sizeDelta.y * .5f - offset);
        }

        private void SetDesiredHeight(float height)
        {
            var sizeGap = content.sizeDelta.y - rectTransform.sizeDelta.y;
            if (sizeGap <= 0) 
                desiredHeight = 0f;
            else 
                desiredHeight = Mathf.Clamp(height, 0, sizeGap);
        }
    }
}


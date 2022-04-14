using System;
using DartCore.Localization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace  DartCore.UI
{
    public class VersionDisplayer : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Version Displayer"), MenuItem("GameObject/UI/DartCore/Version Displayer")]
        public static void AddVersionDisplayer()
        {
            var obj = Instantiate(Resources.Load<GameObject>("Version Displayer"),
                Selection.activeGameObject ? Selection.activeGameObject.transform : null,
                false);
            obj.name = "Version Displayer";
        }
#endif

        #endregion

        public static VersionDisplayer instance;
        
        private TMP_Text versionText;
        private RawImage bg;

        [Header("Behaviour")]
        [SerializeField] private bool dontDestroyOnLoad = true;
        
        [Header("Visuals")]
        [SerializeField] private Color bgColor = Color.red;
        [SerializeField] private string prefixKey;
    
        private void Awake()
        {
            if (instance)
                Destroy(gameObject);
            else if (dontDestroyOnLoad)
                instance = this;

            bg = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            versionText = bg.GetComponentInChildren<TMP_Text>();
            
            UpdateText();
            UpdateColor(bgColor);
            
            if (!dontDestroyOnLoad) return;
            DontDestroyOnLoad(this);
        }

        private void OnEnable() => Localizator.OnLanguageChange += UpdateText;
        private void OnDisable() => Localizator.OnLanguageChange -= UpdateText;
    
        public void UpdateText()
        {
            versionText.text = Localizator.GetString(prefixKey, returnErrorString: false) + " " + Application.version;
        }

        public void UpdateColor(Color color)
        {
            bg.color = color;
        }
    }
}
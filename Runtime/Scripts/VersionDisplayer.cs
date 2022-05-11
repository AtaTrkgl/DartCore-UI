using DartCore.Localization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace  DartCore.UI
{
    [RequireComponent(typeof(CanvasGroup))]
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
        [SerializeField] private KeyCode toggleKey = KeyCode.None;
        [SerializeField] private bool activeOnStart = true;
        
        [Header("Visuals")]
        [SerializeField] private Color bgColor = Color.red;
        [SerializeField, LocalizedKey] private string prefixKey;

        private CanvasGroup canvasGroup;
        
        private void Awake()
        {
            if (instance)
                Destroy(gameObject);
            else if (dontDestroyOnLoad)
                instance = this;

            bg = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            versionText = bg.GetComponentInChildren<TMP_Text>();

            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = activeOnStart ? 1f : 0f;
            
            UpdateText();
            UpdateColor(bgColor);
            
            if (!dontDestroyOnLoad) return;
            DontDestroyOnLoad(this);
        }

        private void OnEnable() => Localizator.OnLanguageChange += UpdateText;
        private void OnDisable() => Localizator.OnLanguageChange -= UpdateText;

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
                canvasGroup.alpha = canvasGroup.alpha > 0f ? 0f : 1f;
        }

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
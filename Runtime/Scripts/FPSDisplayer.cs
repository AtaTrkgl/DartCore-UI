using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace  DartCore.UI
{
    public class FPSDisplayer : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/FPS Displayer", priority=22), MenuItem("GameObject/UI/DartCore/FPS Displayer", priority=22)]
        public static void AddVersionDisplayer()
        {
            var objParent = Selection.activeGameObject != null ? Selection.activeGameObject.transform :
                FindObjectOfType<Canvas>() != null ? FindObjectOfType<Canvas>().transform : null;
            var obj = Instantiate(Resources.Load<GameObject>("FPS Displayer"),
                Selection.activeGameObject ? Selection.activeGameObject.transform : null,
                false);
            obj.name = "FPS Displayer";
        }
#endif

        #endregion

        public static FPSDisplayer instance;
        private static float avg = 0;
        private static float sceneAvg = 0;
        private static int sceneFrameCount = 0;

        [SerializeField] private bool toggled = true;
        [SerializeField] private KeyCode toggleKey = KeyCode.F6;

        [SerializeField] private Color textColor = Color.green;
        private TMP_Text fpsText;
    
        private void Awake()
        {
            if (instance)
                Destroy(gameObject);
            else
                instance = this;
            
            fpsText = GetComponentInChildren<TMP_Text>();
            
            DontDestroyOnLoad(this);
            UpdateColor(textColor);

            SceneManager.activeSceneChanged += (_, __) => { sceneAvg = 0; sceneFrameCount = 0; };
        }

        private void Update()
        {
            if (!fpsText || Time.unscaledDeltaTime == 0f) return;

            if (Input.GetKeyDown(toggleKey)) toggled = !toggled;

            var currentFPS = 1 / Time.unscaledDeltaTime;
            var roundedFPS = Mathf.RoundToInt(currentFPS);
            
            sceneFrameCount++;
            
            avg = (avg * (Time.frameCount - 1) + currentFPS) / Time.frameCount;
            sceneAvg = (sceneAvg * (sceneFrameCount - 1) + currentFPS) / sceneFrameCount;
            
            if (!toggled)
            {
                fpsText.text = "";
                return;
            }
            fpsText.text = $"FPS: {(roundedFPS < 100 ? roundedFPS + " " : roundedFPS.ToString())}"
                           + $" Avg: {Mathf.RoundToInt(avg)}"
                           + $" Scene Avg: {Mathf.RoundToInt(sceneAvg)}";
        }

        public void UpdateColor(Color color)
        {
            textColor = color;
            fpsText.color = color;
        }
    }
}

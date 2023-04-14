using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DartCore.UI
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
        private static float low = 0;
        private static float sceneLow = 0;
        private static int sceneFrameCount = 0;

        [Header("Toggle")]
        [SerializeField] private bool toggled = true;
        [SerializeField] private KeyCode toggleKey = KeyCode.F6;

        [Header("Display Options")] 
        [SerializeField] private bool displayAverage = true;
        [SerializeField] private bool displayLow = true;
        [SerializeField, Tooltip("Only the FPS after this amount of time passes after a level load will be counted as the lowest fps. This helps by avoiding the initial drops on level load, this way you can actually analyze the fps drops mid-level.")]
        private float lowFpsMinTimeSinceLevelLoad = 1f;
        [SerializeField] private bool displaySceneSpecific = true;
        [Space]
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

            SceneManager.activeSceneChanged += (_, __) => { sceneAvg = 0; sceneFrameCount = 0; sceneLow = 0; };
        }

        private void Update()
        {
            if (!fpsText || Time.unscaledDeltaTime == 0f) return;

            if (Input.GetKeyDown(toggleKey)) toggled = !toggled;

            var currentFPS = 1 / Time.unscaledDeltaTime;
            var roundedFPS = Mathf.RoundToInt(currentFPS);

            if (Time.timeSinceLevelLoad > lowFpsMinTimeSinceLevelLoad)
            {
                if (roundedFPS < low || low == 0) low = roundedFPS;
                if (roundedFPS < sceneLow || sceneLow == 0) sceneLow = roundedFPS;
            }

            sceneFrameCount++;

            avg = (avg * (Time.frameCount - 1) + currentFPS) / Time.frameCount;
            sceneAvg = (sceneAvg * (sceneFrameCount - 1) + currentFPS) / sceneFrameCount;

            if (!toggled)
            {
                fpsText.text = "";
                return;
            }

            var textToDisplay = $"FPS: {(roundedFPS < 100 ? roundedFPS + " " : roundedFPS.ToString())}";
            if (displayAverage || displayLow) textToDisplay += " |";
            if (displayAverage) textToDisplay += $" Avg: {Mathf.RoundToInt(avg)}";
            if (displayLow) textToDisplay += $" Low: {Mathf.RoundToInt(low)}";

            if (displaySceneSpecific && (displayAverage || displayLow))
            {
                textToDisplay += " |";
                if (displayAverage) textToDisplay += $" Scene Avg: {Mathf.RoundToInt(sceneAvg)}";
                if (displayLow) textToDisplay += $" Scene Low: {Mathf.RoundToInt(sceneLow)}";
            }

            fpsText.text = textToDisplay;
        }

        public void UpdateColor(Color color)
        {
            textColor = color;
            fpsText.color = color;
        }
    }
}

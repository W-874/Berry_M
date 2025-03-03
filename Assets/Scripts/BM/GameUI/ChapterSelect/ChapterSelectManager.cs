using System.Collections.Generic;
using UnityEngine;
using BM.Utils;
using BM.Utils.Singleton;
using UnityEngine.UI;
using BM.Data;
using BM.Data.ScriptableObject;
using System.Linq;
using BM.GameUI.Global;
using BM.GameUI.Settings;
using BM.GameUI.LevelSelect;
using BM.Global;

namespace BM.GameUI.ChapterSelect
{
    public class ChapterSelectManager : MonoSingleton<ChapterSelectManager>
    {
        [SerializeField] private ChapterShowerControl chapterShowerPrefab;
        [SerializeField] private Transform chapterTransform;
        [SerializeField] private Button backButton, settingButton, startButton;
        [SerializeField] private Image bg, line;
        [SerializeField] private Sprite[] bgImage;

        [Header("Values")]
        [SerializeField, Min(0)] private float scrollSpeed;
        [SerializeField, Min(0)] private float bounceSpeed;
        [SerializeField] private AnimationCurve bounceCurve;
        [SerializeField, Min(0)] private int topOrder = 2;

        public List<ChapterShowerControl> ChapterShowers { get; private set; } = new();
        public static ChapterData[] ChapterData { get; set; }

        private int? nowIndex;
        public int NowIndex
        {
            get => nowIndex ??= 0;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value >= ChapterData.Length)
                {
                    value = ChapterData.Length - 1;
                }
                nowIndex = value;
                if (Instance)
                {
                    Instance.UpdateIndex(value);
                }
            }
        }

        private float? scrollIndex;
        public float ScrollIndex
        {
            get => scrollIndex ??= NowIndex;
            set
            {
                if (value >= -0.5f && value <= ChapterData.Length - 0.5f)
                {
                    scrollIndex = value;
                    if (Mathf.RoundToInt(value) != nowIndex)
                    {
                        NowIndex = Mathf.RoundToInt(value);
                    }
                }
            }
        }

        private float? bounceDuration;
        private float? bounceStartTime;
        private float? bounceStartScrollIndex;

        public static void Init(ChapterDataObject[] obj)
        {
            ChapterData = obj.Select(x => x.CurrentData).ToArray();
        }

        protected override void OnAwake()
        {
            backButton.onClick.AddListener(() => TransitionManager.DoScene(SceneToDo, Color.white, 0.25f));
            settingButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/SettingsScene", Color.white, 0.25f));

            if (ChapterData.IsNullOrEmpty()) return;
            for (var i = 0; i < ChapterData.Length; i++)
            {
                var obj = Instantiate(chapterShowerPrefab.gameObject, chapterTransform);
                var comp = obj.GetComponent<ChapterShowerControl>();
                comp.Init(ChapterData[i]);
                ChapterShowers.Add(comp);
                obj.transform.localPosition = new Vector3(i * 500, i * -200, 0f);
            }

            var lastSelectedStr = PlayerPrefs.GetString("ChapterSelect_Last", null);
            var lastSelectedChapter = string.IsNullOrWhiteSpace(lastSelectedStr)
                ? ChapterData[0]
                : ChapterData.FirstOrDefault(x => x.chapterName.Equals(lastSelectedStr)) ?? ChapterData[0];
            NowIndex = ChapterData.ToList().IndexOf(lastSelectedChapter);

            startButton.onClick.AddListener(IntoLevelSelect);

            SettingsManager.SetSceneToDo("Scenes/ChapterSelectScene");
            MapManager.SetSceneToDo("Scenes/ChapterSelectScene");
            LevelSelectManager.SetSceneToDo("Scenes/ChapterSelectScene");
        }

        private void UpdateIndex(int value)
        {
            PlayerPrefs.SetString("ChapterSelect_Last", ChapterData[value].identifier);
            ChapterShowers[value].transform.SetSiblingIndex(topOrder);
        }

        private void Update()
        {
            UpdateScrollInput();
            UpdateShowerMovement();
        }

        private void UpdateScrollInput()
        {
            if (Input.touchCount > 0)
            {
                var scroll = -Input.touches.Max(x => x.deltaPosition.x);

                if (Mathf.Abs(scroll) > 1f)
                {
                    ScrollIndex += scroll * scrollSpeed / 1000f;
                }

                bounceStartTime = null;
                bounceStartScrollIndex = null;
                bounceDuration = null;
            }
            else
            {
                bounceStartTime ??= Time.time;
                bounceStartScrollIndex ??= ScrollIndex;
                bounceDuration ??= Mathf.Abs(ScrollIndex - NowIndex) / bounceSpeed;
                var progress = bounceCurve.Evaluate(Mathf.Clamp01((Time.time - (float)bounceStartTime) / (float)bounceDuration));
                ScrollIndex = Mathf.Lerp((float)bounceStartScrollIndex, NowIndex, progress);
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ScrollIndex += Input.GetKey(KeyCode.RightArrow) ? 1 : -1;

                Debug.Log(ScrollIndex);

                bounceStartTime = null;
                bounceStartScrollIndex = null;
            }
#endif
        }

        private void UpdateShowerMovement()
        {
            for (var i = 0; i < ChapterShowers.Count; i++)
            {
                var pos = ChapterShowers[i].transform.localPosition;
                var delta = ScrollIndex - i;

                var targetPos = delta * -400f;
                ChapterShowers[i].transform.localPosition = new Vector3(targetPos, delta * 200, delta * 200);
                ChapterShowers[i].transform.localEulerAngles = new Vector3(0, 30 * delta, 0);
                ChapterShowers[i].backGround.color = new Color(1, 1, 1, Mathf.Clamp01(1 - Mathf.Abs(delta * 2)));

                ChapterShowers[i].SetShowerAlpha(Mathf.Clamp01(1 - Mathf.Abs(delta * 0.5f)));
            }

            bg.sprite = bgImage[(int) ScrollIndex];
            line.color = ScrollIndex > 0.5 ? new Color(0, 0.87f, 1f, 1) : new Color(1, 0.78f, 0.82f, 1);
            if ((int)ScrollIndex == 2)
            {
                line.color = new Color(0.7f, 0.5f, 1, 1);
            }
        }

        public void IntoLevelSelect()
        {
            if (ScrollIndex % 1 >= 0.01f) return;
            if (NowIndex == 2)
            {
                TransitionManager.DoScene("Scenes/MapScene", Color.white, 0.25f);
            }
            else
            {
                LevelSelectManager.Init(ChapterData[NowIndex]);
                DataContainers.SetHistoryChapter(NowIndex);
                Debug.Log(NowIndex);
                TransitionManager.DoScene("Scenes/LevelSelectionScene", Color.white, 0.25f);
            }
        }
    }
}
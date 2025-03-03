using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using BM.Data;
using BM.Data.ScriptableObject;
using BM.Gameplay.Managers;
using BM.GameUI.Global;
using BM.GameUI.Result;
using BM.GameUI.Settings;
using BM.Global;
using BM.Utils;
using BM.Utils.Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BM.GameUI.LevelSelect
{
    public class LevelSelectManager : MonoSingleton<LevelSelectManager>
    {
        [Header("Assets")]
        [SerializeField] private LevelShowerControl levelPrefab;

        [Header("Components")]

        [SerializeField] private AudioSource musicSource;

        [SerializeField] private Sprite illustration;
        
        [SerializeField] private Transform levelTransform;

        [SerializeField] private Button backButton, settingsButton, startButton;
        
        [SerializeField] private Button upLevel, downLevel;

        [SerializeField] private Button diffSet;

        [SerializeField] private Canvas _canvas;
         

        [Header("Performances")]
        [SerializeField] private Sprite[] results;

        [Header("Values")]
        [SerializeField] private float scrollSpeed;
        [SerializeField] private float bounceSpeed;
        [SerializeField] private AnimationCurve bounceCurve, musicVolumeCurve;

        public List<LevelShowerControl> LevelShowers { get; private set; } = new();

        private int historyChart = 0;

        public static ChapterData ChapterData { get; set; }
        public static LevelData[] LevelData => ChapterData.levelData;
        
        
        
        //private int? nowIndex;
        /*public int NowIndex
        {
            get => nowIndex ??= 0;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value >= LevelData.Length)
                {
                    value = LevelData.Length - 1;
                }
                nowIndex = value;
                if (Instance)
                {
                    Instance.UpdateIndex(value);
                }
            }
        }*/
        
        public int NowIndex { get; set; }
        public NeregolLevel NowDiff { get; set; }
        
        /*public static Dictionary<string, string> LastSelectedLevels // 章节名，关卡标识符
        {
            get
            {
                if (!PlayerPrefs.HasKey($"LevelSelect_LastLevels")) return null;
                var str = PlayerPrefs.GetString("LevelSelect_LastLevels");
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
            }
            set
            {
                var str = JsonConvert.SerializeObject(value, Formatting.None);
                PlayerPrefs.SetString("LevelSelect_LastLevels", str);
            }
        }*/

        private Coroutine _prevCoroutine;
        private float _prevStartTime;


        private AudioClip[] _previewClips;

        public static void Init(ChapterData chapterData)
        {
            ChapterData = chapterData;
        }

        protected override void OnAwake()
        {
            backButton.onClick.AddListener(() => TransitionManager.DoScene(SceneToDo, Color.black, 0.25f));
            settingsButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/SettingsScene", Color.black, 0.25f));

            if (LevelData.IsNullOrEmpty())
            {
                SceneManager.LoadSceneAsync("Scenes/EntryScene");
                return;
            }
            
            upLevel.onClick.AddListener(() => UpdateLevelShower(-1));
            downLevel.onClick.AddListener(() => UpdateLevelShower(1));

            startButton.onClick.AddListener(StartGameplay);
            diffSet.onClick.AddListener(() => ChangeDiff());

            LevelManager.SetSceneToDo("Scenes/LevelSelectionScene");
            ResultManager.SetSceneToDo("Scenes/LevelSelectionScene");
            SettingsManager.SetSceneToDo("Scenes/LevelSelectionScene");
        }

        private void Start()
        {
            historyChart = DataContainers.GetHistoryChart();
            
            NowIndex = IndexR(historyChart, LevelData);
            
            NowDiff = DiffR(DataContainers.GetHistoryDiff(), LevelData[NowIndex]);

            for (int i = 0; i < LevelData.Length; i++)
            {
                var levelData = LevelData[i];
                int score = DataContainers.Get_RealScore(levelData.songName, NowDiff);
                

                LevelShowerControl shower = Instantiate(levelPrefab.gameObject, levelTransform).GetComponent<LevelShowerControl>();
                shower.Init(levelData);
                LevelShowers.Add(shower);

                if (i != NowIndex)
                {
                    shower.gameObject.SetActive(false);
                }

                shower.BestScore.text = score.ToString();
                shower.ScoreMark.sprite = results[ResultManager.ScoreMark(score)];
            }
            
            UpdateLevelDiffShower();
            
            upLevel.image.sprite = LevelData[IndexR(NowIndex - 1, LevelData)].Illustration;
            downLevel.image.sprite = LevelData[IndexR(NowIndex + 1, LevelData)].Illustration;

            if (LevelData.Length == 1)
            {
                upLevel.gameObject.transform.parent.gameObject.SetActive(false);
                downLevel.gameObject.transform.parent.gameObject.SetActive(false);
            }
            _previewClips = new AudioClip[LevelData.Length];
            StartCoroutine(LoadPreviewClips());

            //var dict = LastSelectedLevels ?? new Dictionary<string, string>();
            var levels = LevelData.Select(x => x.songID).ToList();

            /*if (dict.ContainsKey(ChapterData.chapterName))
            {
                // NowIndex = levels.IndexOf(dict[ChapterData.chapterName]);
            }
            else
            {
                // dict.Add(ChapterData.chapterName, levels[NowIndex]);
            }*/

            //LastSelectedLevels = dict;

            //UpdateIndex(NowIndex);
            ChangeMusicPreview();

            //LevelSelectHarder.Instance.OnStart();
        }

        /*public void UpdateIndex(int index)
        {
            var levelData = LevelData[index];
            
            var dict = LastSelectedLevels;
            dict[ChapterData.chapterName] = LevelData[NowIndex].songID;
            LastSelectedLevels = dict;
        }*/

        private void Update()
        {
            UpdateScrollInput();
            //UpdateShowerMovement();
        }

        private IEnumerator LoadPreviewClips()
        {
            var index = NowIndex;
            for (var i = index; i < _previewClips.Length; i++)
            {
                var clip = LevelData[i].GetSongClip();
                _previewClips[i] = clip;
            }
            if (index <= 0) yield break;
            for (var i = index - 1; i >= 0; i--)
            {
                var clip = LevelData[i].GetSongClip();
                _previewClips[i] = clip;
            }
        }

        private int IndexR(int index, LevelData[] data)
        {
            int len = data.Length;

            if (index > len - 1)
            {
                index = 0;
            }else if (index < 0)
            {
                index = len - 1;
            }

            return index;

        }

        private void ChangeDiff()
        {
            var nowDiff = (NeregolLevel)((int)NowDiff << 1);
            NowDiff = nowDiff;
            UpdateLevelDiffShower();
        }

        private void UpdateLevelShower(int type)
        {
            LevelShowers[NowIndex].gameObject.SetActive(false);
            NowIndex = IndexR((NowIndex + type), LevelData);
            UpdateLevelDiffShower();
            LevelShowers[NowIndex].gameObject.SetActive(true);


            upLevel.image.sprite = LevelData[IndexR(NowIndex - 1,LevelData)].Illustration;
            downLevel.image.sprite = LevelData[IndexR(NowIndex + 1, LevelData)].Illustration;
            
            ChangeMusicPreview();
            
            DataContainers.SetHistoryChart(NowIndex);
        }

        private void UpdateLevelDiffShower()
        {
            var level = LevelShowers[NowIndex];
            var data = LevelData[NowIndex];
            
            NowDiff = DiffR(NowDiff, data);
            
            DataContainers.SetHistoryDiff(NowDiff);
            var diff = data.levelDifficulty[NowDiff.Index()];
            level.SetDiff(NowDiff.Abbr());
            level.SetDiffNum(diff);
            level.SetChartArtist(NowDiff.Index(), data);

            var score = DataContainers.Get_RealScore(data.songName, NowDiff);
            level.BestScore.text = score.ToString();
            level.ScoreMark.sprite = results[ResultManager.ScoreMark(score)];

        }

        private NeregolLevel DiffR(NeregolLevel from, LevelData data)
        {
            
            if (from > data.levelDiffs.Max())
            {
                from = data.levelDiffs.Min();
            }

            if (from < data.levelDiffs.Min())
            {
                from = data.levelDiffs.Min();
            }

            return from;
        }

        private void ChangeMusicPreview()
        {
            if (_prevCoroutine != null) StopCoroutine(_prevCoroutine);

            musicSource.Pause();

            //TODO:加个选歌音效

            _prevCoroutine = StartCoroutine(CO_UpdateMusicVolume());
        }

        private IEnumerator CO_UpdateMusicVolume()
        {

            var levelData = LevelData[NowIndex];

            yield return new WaitUntil(() => _previewClips[NowIndex] != null);

            musicSource.clip = _previewClips[NowIndex]; //levelData.musicClip;

            const float appearTime = 1.75f; // 淡入淡出时间
            const float delayTime = 0.25f; // 停止缓冲时间

            var startTime = levelData.previewTime.x;
            var endTime = levelData.previewTime.y;

            if (startTime <= 0f)
            {
                print("音频开始预览时间有毛病, 已自动调节");
                startTime = musicSource.clip.length / 2f - appearTime - 5f;
            }

            if (endTime - startTime < appearTime * 2f)
            {
                print("音频结束预览时间有毛病, 已自动调节");
                endTime = startTime + appearTime * 2f + 5f;
            }

            var length = endTime - startTime;

            yield return new WaitForSecondsRealtime(delayTime);

            PlayMusicPreview(startTime);

            while (true)
            {
                var duration = Time.time - _prevStartTime;

                if (duration <= length)
                {
                    if (duration < appearTime)
                    {
                        musicSource.volume = musicVolumeCurve.Evaluate(duration / appearTime);
                    }
                    else if (duration > length - appearTime)
                    {
                        musicSource.volume = musicVolumeCurve.Evaluate((length - duration) / appearTime);
                    }
                    else
                    {
                        musicSource.volume = 1f;
                    }
                }
                else
                {
                    PlayMusicPreview(startTime);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private void PlayMusicPreview(float startTime)
        {
            _prevStartTime = Time.time;
            musicSource.Pause();
            musicSource.time = startTime;
            musicSource.Play();
        }

        
        private void UpdateScrollInput()
        {
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UpdateLevelShower(1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UpdateLevelShower(-1);
            }
        }

        private void StartGameplay()
        {
            var levelData = LevelData[NowIndex];
            
            if (!levelData.levelDiffs.HasFlag(LevelSelectHarder.CurrentDiffs)) return;

            StartCoroutine(StartAnimation(levelData));

            // musicSource.Pause();
            // levelData.usingLevelDiffs = LevelSelectHarder.CurrentDiffs;
            //
            // levelData.TargetDiff = NowDiff;
            // ChartSelectManager.StartGame(levelData,ChapterData);
            // TransitionManager.DoScene("Scenes/GameplayScene", Color.black, 0.25f, 0.5f, 0.5f);
        }

        private IEnumerator StartAnimation(LevelData levelData)
        {
            var showerAnimation = LevelShowers[NowIndex].gameObject.GetComponent<Animation>();
            var canvasAnimation = _canvas.GetComponent<Animation>();
            
            showerAnimation.wrapMode = WrapMode.Once;
            canvasAnimation.wrapMode = WrapMode.Once;

            canvasAnimation.Play();
            showerAnimation.Play();

            while (canvasAnimation.isPlaying || showerAnimation.isPlaying)
            {
                yield return new();
            }

            musicSource.Pause();
            levelData.usingLevelDiffs = LevelSelectHarder.CurrentDiffs;

            levelData.TargetDiff = NowDiff;//TODO,set targetdiffent
            ChartSelectManager.StartGame(levelData,ChapterData);
            TransitionManager.DoScene("Scenes/GameplayScene", Color.black, 0.25f, 0.5f, 0.5f);
        }
    }
}

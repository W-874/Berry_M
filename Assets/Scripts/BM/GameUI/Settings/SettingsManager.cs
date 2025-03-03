using System.Linq;
//using BM.Gameplay.Managers;
using BM.GameUI.Global;
using BM.Global;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BM.GameUI.Settings
{
    public class SettingsManager : MonoSingleton<SettingsManager>
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private Transform baseTransform;
        [SerializeField] private Button backButton, toAudioSettings, toGameplaySettings, toOthersSettings;
        [SerializeField] private Image toAudioBg, toGameplayBg, toOthersBg;
        [SerializeField] private GameObject audioSettings, gameplaySettings, othersSettings;
        [SerializeField] private InputField playerNameInput;

        [SerializeField] private Text playerNameText;
        //[SerializeField] private Button tutorialButton, offsetButton;
        [SerializeField] private HugeSlider mainVolume, musicVolume, hitSoundVolume, uiSoundVolume, noteSpeed, hitFxSize,lighting; //globalNoteSpeed, dspBufferSize;
        [SerializeField] private HugeSlider Offest, Mask_;
        //[SerializeField] private Button dspBufferButton;
        [SerializeField] private SwitchButton fullScreenJudgeButton, onDemandJudgmentButton, cameraResuxButton;

        [Header("TEST")]
        [SerializeField] private HugeSlider Test;
        [SerializeField] private HugeSlider JudgeAngle;
        [SerializeField] private HugeSlider JudgeLine;

        private GlobalSettings globalSettings;

        protected override void OnAwake()
        {
            globalSettings = GlobalSettings.CurrentSettings;

            InitToggle();

            playerNameInput.text = globalSettings.PlayerName;
            //_brightness = _globalSettings.BrightnessLevel;
            //SetBrightnessText();

            mainVolume.CurrentValue = globalSettings.MainVolume;
            musicVolume.CurrentValue = globalSettings.MusicVolume;
            hitSoundVolume.CurrentValue = globalSettings.HitSoundVolume;
            uiSoundVolume.CurrentValue = globalSettings.UISoundVolume;
            noteSpeed.CurrentValue = globalSettings.GlobalNoteSpeed;
            // globalNoteSpeed.CurrentValue = _globalSettings.GlobalNoteSpeed;
             hitFxSize.CurrentValue = globalSettings.HitFxSize;
            lighting.CurrentValue = globalSettings.Lighting;
            Test.CurrentValue = globalSettings.IsTest;
            // dspBufferSize.CurrentValue = _globalSettings.DSPBufferSize;
            Offest.CurrentValue = globalSettings.MusicOffset;

            playerNameInput.onEndEdit.AddListener(x => globalSettings.PlayerName = x);
            //offsetButton.onClick.AddListener(IntoOffset);
            //tutorialButton.onClick.AddListener(IntoTutorial);

            Test.CurrentValue = globalSettings.IsTest;
            Mask_.CurrentValue = globalSettings.Mask;
            JudgeAngle.CurrentValue = globalSettings.JudgeAngle;
            JudgeLine.CurrentValue = globalSettings.JudgeLine;

            mainVolume.AddSlideListener(_ =>
            {
                globalSettings.MainVolume = mainVolume.CurrentValue;
                source.volume = mainVolume.CurrentValue;
            });
            musicVolume.AddSlideListener(_ => globalSettings.MusicVolume = musicVolume.CurrentValue);
            hitSoundVolume.AddSlideListener(_ => globalSettings.HitSoundVolume = hitSoundVolume.CurrentValue);
            uiSoundVolume.AddSlideListener(_ => globalSettings.UISoundVolume = uiSoundVolume.CurrentValue);
            noteSpeed.AddSlideListener(_ => globalSettings.GlobalNoteSpeed = noteSpeed.CurrentValue);
            // globalNoteSpeed.AddSlideListener(_ => _globalSettings.GlobalNoteSpeed = globalNoteSpeed.CurrentValue);
            hitFxSize.AddSlideListener(_ => globalSettings.HitFxSize = hitFxSize.CurrentValue);
            lighting.AddSlideListener(_ => globalSettings.Lighting = lighting.CurrentValue);
            // dspBufferSize.AddSlideListener(_ => _globalSettings.DSPBufferSize = (int)dspBufferSize.CurrentValue);

            Offest.AddSlideListener(_ => globalSettings.MusicOffset = Offest.CurrentValue);
            Mask_.AddSlideListener(_ => globalSettings.Mask = Mask_.CurrentValue);

            Test.AddSlideListener(_ => globalSettings.IsTest = (byte)Test.CurrentValue);
            JudgeAngle.AddSlideListener(_ => globalSettings.JudgeAngle = (int)JudgeAngle.CurrentValue);
            JudgeLine.AddSlideListener(_ => globalSettings.JudgeLine = (int)JudgeLine.CurrentValue);

            //dspBufferButton.onClick.AddListener(ChangeDSPBuffer);

            backButton.onClick.AddListener(Back);


            toAudioSettings.onClick.AddListener(ToAudioSettings);
            toGameplaySettings.onClick.AddListener(ToGameplaySettings);
            toOthersSettings.onClick.AddListener(ToOthersSettings);
        }

        void IsOpenTestMode()
        {
            //byte0 ??????
            //byte1 ??????
            //byte2 ??????????
            //byte3 ??????bad????

        }

        public void Back()
        {
            SaveSettings();
            //HitSoundManager.Init();
            TransitionManager.DoScene(SceneToDo, Color.black, 0.25f);
        }

        public void SaveSettings()
        {
            GlobalSettings.CurrentSettings = globalSettings;
        }

        public void SetNameEnd()
        {
            globalSettings.PlayerName = playerNameInput.text;
            playerNameText.text = playerNameInput.text;
            playerNameInput.gameObject.SetActive(false);
        }

        public void SetNameStart()
        {
            playerNameInput.gameObject.SetActive(true);
            playerNameInput.text = globalSettings.PlayerName;
        }

        private void IntoOffset()
        {
            GlobalSettings.CurrentSettings = globalSettings;
            //HitSoundManager.Init();
            TransitionManager.DoScene("Scenes/OffsetScene", Color.black, 0.25f);
        }

        // private void IntoTutorial()
        // {
        //     var levelData = tutorial.CurrentData;

        //     var path = $"Levels/{levelData.PathFather}/{levelData.identifier}/";

        //     var chartAsset = Resources.Load<TextAsset>($"{path}Chart_ST");
        //     if (chartAsset == null) return;

        //     levelData.UsingIllustration = Resources.Load<Sprite>($"{path}Illustration");
        //     levelData.UsingMusicClip = Resources.Load<AudioClip>($"{path}Music");
        //     levelData.UsingLevelHarder = LevelHarder.Start;
        //     levelData.UsingChart = ChartUtility.ReadChart(chartAsset.text);

        //     LevelManager.LevelData = levelData;

        //     LevelManager.SetSceneToDo("Scenes/ChapterSelectScene");
        //     ResultManager.SetSceneToDo("Scenes/ChapterSelectScene");

        //     TransitionManager.DoScene("Scenes/GameScene", Color.black, 0.25f);
        // }

        private void ChangeDSPBuffer()
        {
            var config = AudioSettings.GetConfiguration();
            config.dspBufferSize = Mathf.RoundToInt(globalSettings.DSPBufferSize);
            Debug.Log($"Set DSP Buffer to {Mathf.RoundToInt(globalSettings.DSPBufferSize)}");
            AudioSettings.Reset(config);
            source.PlayScheduled(AudioSettings.dspTime);
        }

        private void Update()
        {
            if (Input.touches.Length > 0 && !EventSystem.current.IsPointerOverGameObject())
            {
                var finger = Input.touches.OrderByDescending(x => x.deltaPosition.y).First();

                const float maxHeight = 600f;

                if (!EventSystem.current.IsPointerOverGameObject(finger.fingerId))
                {
                    var scroll = finger.deltaPosition.y;
                    if (Mathf.Abs(scroll) > 0.1f && baseTransform.localPosition.y is >= 0 and <= maxHeight)
                    {
                        baseTransform.localPosition += new Vector3(0f, scroll * 1080f / Screen.height, 0f);
                    }
                    if (baseTransform.localPosition.y < 0) baseTransform.localPosition = Vector3.zero;
                    if (baseTransform.localPosition.y > maxHeight) baseTransform.localPosition = new Vector3(0f, maxHeight, 0f);
                }
            }
        }

        private void ToAudioSettings()
        {
            toAudioSettings.image.color = new Color(0f, 1f, 1f, 0.47059f);
            toAudioSettings.enabled = false;
            toAudioBg.color = new Color(1f, 0f, 0f, 0.47059f);

            toGameplaySettings.image.color = new Color(1f, 0f, 0f, 0.47059f);
            toGameplaySettings.enabled = true;
            toGameplayBg.color = new Color(0f, 1f, 1f, 0.47059f);

            toOthersSettings.image.color = new Color(1f, 0f, 0f, 0.47059f);
            toOthersSettings.enabled = true;
            toOthersBg.color = new Color(0f, 1f, 1f, 0.47059f);

            audioSettings.SetActive(true);
            gameplaySettings.SetActive(false);
            othersSettings.SetActive(false);

            globalSettings.NowWhichSettings = 0;
        }

        private void ToGameplaySettings()
        {
            toAudioSettings.image.color = new Color(1f, 0f, 0f, 0.47059f);
            toAudioSettings.enabled = true;
            toAudioBg.color = new Color(0f, 1f, 1f, 0.47059f);

            toGameplaySettings.image.color = new Color(0f, 1f, 1f, 0.47059f);
            toGameplaySettings.enabled = false;
            toGameplayBg.color = new Color(1f, 0f, 0f, 0.47059f);

            toOthersSettings.image.color = new Color(1f, 0f, 0f, 0.47059f);
            toOthersSettings.enabled = true;
            toOthersBg.color = new Color(0f, 1f, 1f, 0.47059f);

            audioSettings.SetActive(false);
            gameplaySettings.SetActive(true);
            othersSettings.SetActive(false);

            globalSettings.NowWhichSettings = 1;
        }

        private void ToOthersSettings()
        {
            toAudioSettings.image.color = new Color(1f, 0f, 0f, 0.47059f);
            toAudioSettings.enabled = true;
            toAudioBg.color = new Color(0f, 1f, 1f, 0.47059f);

            toGameplaySettings.image.color = new Color(1f, 0f, 0f, 0.47059f);
            toGameplaySettings.enabled = true;
            toGameplayBg.color = new Color(0f, 1f, 1f, 0.47059f);

            toOthersSettings.image.color = new Color(0f, 1f, 1f, 0.47059f);
            toOthersSettings.enabled = false;
            toOthersBg.color = new Color(1f, 0f, 0f, 0.47059f);

            audioSettings.SetActive(false);
            gameplaySettings.SetActive(false);
            othersSettings.SetActive(true);

            globalSettings.NowWhichSettings = 2;
        }

        private void InitToggle()
        {
            switch (globalSettings.NowWhichSettings)
            {
                case 0:
                    ToAudioSettings();
                    break;
                case 1:
                    ToGameplaySettings();
                    break;
                case 2:
                    ToOthersSettings();
                    break;
            }


        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using BM.Data;
using UnityEngine;
using BM.Data.ScriptableObject;
using BM.GameUI.ChapterSelect;
using BM.Utils;
using BM.Gameplay.Managers;
using BM.Utils.Singleton;
using UnityEngine.UI;
using BM.Global;
using BM.GameUI.Global;
using BM.GameUI.LevelSelect;
using BM.GameUI.Main;
using DG.Tweening;

namespace BM.GameUI.Entry
{
    public class EntryManager : MonoSingleton<EntryManager>
    {
        public AudioSource audioSource;
        public Text versionShow, copyright;
        public Image blackMask;

        [SerializeField] private ChapterDataObject[] chapterDataObjects;
        [SerializeField] private CharaDataObject[] charaDataObjects;


        private bool isDone;
        private int waitFrames;


        protected override async void OnAwake()
        {
#if !UNITY_EDITOR
        Application.targetFrameRate = 300;
#endif
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            blackMask.gameObject.SetActive(true);

            var config = AudioSettings.GetConfiguration();
            var dsp = GlobalSettings.CurrentSettings.DSPBufferSize;
            if (dsp == 0)//首次启动时自动获取系统dspbuffer并保存！
            {
                AudioSettings.GetDSPBufferSize(out var length, out var num);
                dsp = GlobalSettings.CurrentSettings.DSPBufferSize = length;
            }
            config.dspBufferSize = Mathf.RoundToInt(dsp);
            AudioSettings.Reset(config);

            versionShow.text = $"Berry Melody - V{Application.version} , Init DSP Buffer - {dsp}";

            await Task.Delay(500);

            if (blackMask)
            {
                blackMask.DoFade(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    blackMask.gameObject.SetActive(false);
                });
            }

            audioSource.Play();
        }


        private void Start()
        {
            ChapterSelectManager.Init(chapterDataObjects);
            MainManager.Init(charaDataObjects);
            HitSoundManager.Init();
        }

        void Update()
        {

            waitFrames++;
            if (Input.GetMouseButtonUp(0) && waitFrames >= 10)
            {
                if (!isDone)
                {
                    Into();
                }
            }

        }

        private void Into()
        {
            isDone = true;

            audioSource.DoVolume(0f, 1f);

            // if (PlayerPrefs.GetInt("SecondClosedBeta_Tutorial", 0) != 1)
            // {
            //     PlayerPrefs.SetInt("SecondClosedBeta_Tutorial", 1);
            //     IntoTutorial();
            //     return;
            // }

            TransitionManager.DoScene("Scenes/MainScene", Color.white, 1f);
        }
    }
}
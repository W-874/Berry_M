using System;
using BM.Data;
using BM.Gameplay.Managers;
//using BM.Gameplay.Managers;
using BM.GameUI.Global;
using BM.GameUI.Settings;
using BM.Global;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BM.GameUI.Result
{
    [DisallowMultipleComponent]
    public class ResultManager : MonoSingleton<ResultManager>
    {
        [SerializeField, Header("清晰曲绘")] private Image bg, illustration;

        [SerializeField] private Text musicName, composerName, diffNum, difficulty;
        [SerializeField] private Text perfect, good, bad, miss;
        [SerializeField] private Text  nowScore;
        [SerializeField] private Image ResultMark;
        [SerializeField] private Image partner;

        [SerializeField, Header("评级贴图从FALSE放到DEMO")] private Sprite[] results;

        [SerializeField] private Button backButton, restartButton;

        public static ResultData ResultData { get; set; }
        public static int BestScore { get; set; }

        public static LevelData LevelData { get; set; }
        
        public static CharaData CharaData { get; set; }
        
        
        public static void Init(ResultData last, LevelData from,int score,bool IsNewBest)
        {
            ResultData = last;
            LevelData = from;

            BestScore = score;
            
            if (IsNewBest)
            {
                DataContainers.UpdataBest(from.songName, from.TargetDiff, last.ND);
                DataContainers.Updata(from.songName,from.TargetDiff,last.judgeList);
                DataContainers.Save(from.songName, from.TargetDiff);
            }
        }

        public static void InitPartner(CharaData charaDate)
        {
            CharaData = charaDate;
        }

        protected override void OnAwake()
        {
            if (LevelData == null) return;

            //illustration.sprite = LevelData.UsingIllustration[0];
            musicName.text = LevelData.songName;
            composerName.text = LevelData.songArtist;
            bg.sprite = LevelData.Illustration;
            perfect.text = $"{ResultData.PerfectNum}";
            good.text = $"{ResultData.GreatNum}";
            bad.text = $"{ResultData.BadNum}";
            miss.text = $"{ResultData.MissNum}";
            nowScore.text = $"{ResultData.Score}";
            ResultMark.sprite = results[ScoreMark(ResultData.Score)];
            if (CharaData != null)
                partner.sprite = CharaData._Sprite;

            difficulty.text = LevelData.TargetDiff.Abbr();
            double num = (double)LevelData.levelDifficulty[LevelData.TargetDiff.Index()];
            double numc = Math.Floor(num);

            if (Math.Abs(num - numc) >= 0.5)
            {
                diffNum.text = numc.ToString() + "+";
            }
            else diffNum.text = numc.ToString();
            
            SettingsManager.SetSceneToDo("Scenes/ResultScene");
            backButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/LevelSelectionScene", Color.black, 0.25f));
            restartButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/GameplayScene", Color.black, 0.25f));
        }
        
        

        public static int ScoreMark(int score)
        {
            double nowIndex = 0;
            int index = 0;
            nowIndex = (double) score / 100000;

            return index = nowIndex switch
            {
                < 8 => 0,
                >= 8 and < 9 => 1,
                >= 9 and < 9.4 => 2,
                >= 9.4 and < 9.6 => 3,
                >= 9.6 and < 9.9 => 4,
                >= 9.9 => 5,
                _ => 0
            };
        }
        
    }
}

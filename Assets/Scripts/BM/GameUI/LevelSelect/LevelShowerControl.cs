using System;
using System.Globalization;
using BM.Data;
using BM.Global;
using Main;
using Map;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace BM.GameUI.LevelSelect
{
    public class LevelShowerControl : MonoBehaviour
    {
        [SerializeField] private Image illustration;
        [SerializeField] private Text songName, songArt, diff, chartArt, illusArt;
        [SerializeField] private Image backGround;
        [SerializeField] private Text diffNum;
        [SerializeField] private Text score;
        [SerializeField] private Image scoreMark;
        public Text BestScore { get; set; }

        
        public Image ScoreMark { get; set; }

        //private Color skyBlue = new Color(0f, 1f, 1f, 0.47059f);
        //private Color red = new Color(1f, 0f, 0f, 0.47059f);

        private const float Duration = 0.25f;
        //private ChapterData _chapterData;

        public void Init(LevelData levelData)
        {
            songName.text = levelData.songName;
            songArt.text = levelData.songArtist;
            //chartArt.text = "谱师：" + levelData.chartArtist;
            illusArt.text = "曲绘：" + levelData.illusArtist;

            BestScore = score;
            ScoreMark = scoreMark;

            illustration.sprite = levelData.selectIll;
            backGround.sprite = levelData.Illustration;

            var historyDiff = DataContainers.GetHistoryDiff();

            if (historyDiff > levelData.levelDiffs.Max())
            {
                var diff = levelData.levelDiffs.Max().Abbr();
                double num = (double)levelData.levelDifficulty[levelData.levelDiffs.Max().Index()];
                int index = levelData.levelDiffs.Max().Index();
                
                SetDiff(diff);
                SetDiffNum(num);
                SetChartArtist(index, levelData);
            }
            else if (historyDiff < levelData.levelDiffs.Min())
            {
                var diff = levelData.levelDiffs.Min().Abbr();
                double num = (double)levelData.levelDifficulty[levelData.levelDiffs.Min().Index()];
                int index = levelData.levelDiffs.Min().Index();

                SetDiff(diff);
                SetDiffNum(num);
                SetChartArtist(index, levelData);
            }
            else
            {
                var diff = historyDiff.Abbr();
                double num = (double)levelData.levelDifficulty[historyDiff.Index()];
                int index = historyDiff.Index();
                
                SetDiff(diff);
                SetDiffNum(num);
                SetChartArtist(index, levelData);
            }
            
        }
        

        public void SetDiff(string str)
        {
            diff.text = str;
        }

        public void SetDiffNum(double num)
        {
            double numc = Math.Floor(num);
            
            if (Math.Abs(num - numc) >= 0.5)
            {
                diffNum.text = numc.ToString() + "+";
            }
            else diffNum.text = numc.ToString();
        }

        public void SetChartArtist(int index, LevelData levelData)
        {
            chartArt.text = "谱师：" + levelData.chartArtist[index];
        }

        // public void SetDifficultyImage(LevelHarder diff)
        // {
        //     difficultyImage.sprite = difficultyTextures[diff.Index()];
        // }

    }
}

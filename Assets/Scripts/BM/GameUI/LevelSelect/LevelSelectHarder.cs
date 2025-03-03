using BM.Data;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace BM.GameUI.LevelSelect
{
    public class LevelSelectHarder : MonoSingleton<LevelSelectHarder>
    {

        private static NeregolLevel _levelDiffs;
        public static NeregolLevel CurrentDiffs
        {
            get => _levelDiffs;
            private set => _levelDiffs = value;
        }

        public void OnStart()
        {
            var lastHarder = PlayerPrefs.GetInt("LevelSelect_LastHarder", 1);
            _levelDiffs = (NeregolLevel) lastHarder;
            ChangeHarder(_levelDiffs);
        }

        public void AddHarder()
        {
            //if (LevelSelectManager.Instance.ScrollIndex % 1 >= 0.01f) return;
            
            var nowHarder = NeregolLevel.Illusion;
            var harder = LevelSelectManager.LevelData[LevelSelectManager.Instance.NowIndex].levelDiffs;
            
            if (CurrentDiffs < harder.Max())
            {
                nowHarder = (NeregolLevel) ((int) CurrentDiffs << 1);
            }
            
            if (CurrentDiffs >= harder.Max())
            {
                nowHarder = harder.Min();
            }
        
            ChangeHarder(nowHarder);
        }

        private void ChangeHarder(NeregolLevel levelHarder)
        {
            CurrentDiffs = levelHarder;
            //LevelSelectManager.Instance.UpdateIndex(LevelSelectManager.Instance.NowIndex);
            PlayerPrefs.SetInt("LevelSelect_LastHarder", (int) CurrentDiffs);
        }

        public void UpdateShower()
        {
            for (var i = 0; i < LevelSelectManager.Instance.LevelShowers.Count; i++)
            {
                var level = LevelSelectManager.Instance.LevelShowers[i];
                var data = LevelSelectManager.LevelData[i];
                var diff = data.levelDiffs.HasFlag(CurrentDiffs)
                    ? "data.levelDifficulty[CurrentDiffs.Index()]"
                    : "-";
                level.SetDiff(CurrentDiffs.Abbr());
                //level.SetDiffNum(diff);
                //level.SetDifficultyImage(CurrentHarder);
            }
        }
    }
}

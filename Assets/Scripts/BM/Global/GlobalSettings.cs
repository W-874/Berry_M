using BM.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace BM.Global
{
    public class GlobalSettings
    {
        [JsonIgnore]
        public static GlobalSettings CurrentSettings
        {
            get
            {
                if (!PlayerPrefs.HasKey("Global_Settings")) return new GlobalSettings();
                var str = PlayerPrefs.GetString("Global_Settings");
                return JsonConvert.DeserializeObject<GlobalSettings>(str);
            }
            set
            {
                var str = JsonConvert.SerializeObject(value, Formatting.None);
                PlayerPrefs.SetString("Global_Settings", str);
            }
        }

        public int NowWhichSettings { get; set; }//0 Audio，1 Gameplay，2 Others
        public string PlayerName { get; set; }//玩家id，后续迁移到联网
        public float MusicOffset { get; set; }//音频偏移
        public float TouchOffset { get; set; }//触摸偏移
        public float MainVolume { get; set; }//主音量
        public float MusicVolume { get; set; }//音乐音量
        public float HitSoundVolume { get; set; }//打击音量
        public float UISoundVolume { get; set; }//背景UI音量
        public bool ThreeDSound { get; set; }
        public float GlobalNoteSpeed { get; set; }//下落速度
        public float HitFxSize { get; set; }//打击特效大小
        public int DSPBufferSize { get; set; }
        public float Lighting { get; set; }//谱面亮度
        public float Mask { get; set; }//遮罩

        public bool FirstPlay { get; set; }//首次游玩

        public byte IsTest { get; set; }//调试模式
        public int JudgeAngle { get; set; }//调试模式判定角度
        public int JudgeLine { get; set; }//调试模式判定角度

        public int HistoryChapter { get; set; }//历史选章
        public int HistoryChart { get; set; } //历史选曲

        public NeregolLevel HistoryDiff { get; set; }
        
        public int Partner { get; set; }
        public GlobalSettings()
        {
            NowWhichSettings = 0;
            PlayerName = "Player";
            MusicOffset = 0f;
            TouchOffset = 0f;
            MainVolume = 1f;
            MusicVolume = 1f;
            HitSoundVolume = 1f;
            UISoundVolume = 1f;
            ThreeDSound = false;
            GlobalNoteSpeed = 1f;
            HitFxSize = 1f;
            DSPBufferSize = 0;
            Lighting = 1;

            FirstPlay = true;

            IsTest = 0;
            Mask = 0.1f;
            JudgeAngle = 28;
            JudgeLine = 180;
            HistoryChapter = 0;
            HistoryChart = 0;
            HistoryDiff = NeregolLevel.Reality;

            Partner = 0;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace BM.Data
{
    [Serializable]
    public class LevelData
    {
        [Header("Song Infos")] 
        public string songName; //歌曲名称
        public string chapterID;//章节ID同谱面文件夹名
        public string songID; //歌曲ID
        public string songArtist; //曲师
        public string illusArtist; //画师
        public string[] chartArtist; //谱师
        public Vector2 previewTime; //预览时间
        
        [Header("Multiply Infos")]
        public float[] levelDifficulty;
        public NeregolLevel levelDiffs; //谱面差分
        [NonSerialized] public NeregolLevel usingLevelDiffs;
        /*public TextAsset Reality;
        public TextAsset Illusion;
        public TextAsset Twist;
        public TextAsset Ruin;
        public TextAsset Neregol;*/
        public Sprite Illustration, selectIll; //曲绘
        public AudioClip MusicClip; //音乐
        
        [NonSerialized] public string PathFather; //我也不知道这啥

        public NeregolLevel TargetDiff = 0;
        public string GetPath()
        {
            Debug.Log("Chart/" + chapterID.ToString() + "/" + songName + "/" + Enum.GetName(typeof(NeregolLevel), TargetDiff));
            return "Chart/" + chapterID.ToString() + "/" + songName +"/"+ Enum.GetName(typeof(NeregolLevel), TargetDiff);
        }
        public AudioClip GetSongClip()
        {
            return MusicClip;
        }
    }

    [Flags]
    public enum NeregolLevel
    {
        Reality = 1,//简单的难度/miss
        Illusion = 1<<1,//中等的难度/bad
        Twist = 1<<2,//困难的难度/100ms
        Ruin = 1<<3,// 特困难难度/50ms
        Neregol = 1<<4//不规则难度/16ms
    }

    public static class LevelDataHelper
    {
        public static int Index(this NeregolLevel diffs)
        {
            return diffs switch
            {
                NeregolLevel.Reality => 0,
                NeregolLevel.Illusion => 1,
                NeregolLevel.Twist => 2,
                NeregolLevel.Ruin => 3,
                NeregolLevel.Neregol => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(diffs), diffs, "Level Harder is invalid")
            };
        }

        public static string Abbr(this NeregolLevel diffs)
        {
            return diffs switch
            {
                NeregolLevel.Reality => "RL",
                NeregolLevel.Illusion => "IS",
                NeregolLevel.Twist => "TT",
                NeregolLevel.Ruin => "RU",
                NeregolLevel.Neregol => "ND",
                _ => "??"
            };
        }

        public static NeregolLevel Max(this NeregolLevel harder)
        {
            if (harder.HasFlag(NeregolLevel.Ruin)) return NeregolLevel.Ruin;
            if (harder.HasFlag(NeregolLevel.Twist)) return NeregolLevel.Twist;
            if (harder.HasFlag(NeregolLevel.Illusion)) return NeregolLevel.Illusion;
            if (harder.HasFlag(NeregolLevel.Reality)) return NeregolLevel.Reality;
            return NeregolLevel.Neregol;
        }

        public static NeregolLevel Min(this NeregolLevel harder)
        {
            if (harder.HasFlag(NeregolLevel.Reality)) return NeregolLevel.Reality;
            if (harder.HasFlag(NeregolLevel.Illusion)) return NeregolLevel.Illusion;
            if (harder.HasFlag(NeregolLevel.Reality)) return NeregolLevel.Reality;
            if (harder.HasFlag(NeregolLevel.Ruin)) return NeregolLevel.Ruin;
            return NeregolLevel.Neregol;
        }

        public static string GetDifficultyName(float diff)
        {
            return diff switch
            {
                0 => "Unfinished",
                -1 => "?",
                _ => $"{diff}"
            };
        }
    }
    
    
}
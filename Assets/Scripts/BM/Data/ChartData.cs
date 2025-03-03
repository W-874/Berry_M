using System;
using BM.Utils;

namespace BM.Data
{
    /// <summary> 整个谱面数据（除去关卡数据） </summary>
    [Serializable]
    public class ChartData
    {
        public int noteCount;
        public TrackData[] tracks;
    }

    /// <summary> 缓动事件结构定义 </summary>
    //依次为：开始时间、结束时间、开始值、结束值、缓动类型。
    [Serializable]
    public class EachEvent
    {
        public float startTime;
        public float endTime;
        public float startValue;
        public float endValue;
        public EaseUtility.Ease moveType;
        public float startRange;
        public float endRange;
    }


    /// <summary> 每条轨道的数据 </summary>
    [Serializable]
    public class TrackData
    {
        public NoteData[] notes;
        public EachEvent[] noteSpeedEvents;
    }

    /// <summary> Note数据结构 </summary>
    [Serializable]
    public class NoteData
    {
        public float judgeTime;
        public JudgeType type;
        public float holdTime;
        public float moveSpeed;

        public bool IsClick => type == JudgeType.Tap;
        public bool IsDrag => type == JudgeType.Drag;
        public bool IsHold => type == JudgeType.Hold;
        public bool IsFlick => type == JudgeType.Flick;
    }

    /// <summary> 判定类型 </summary>
    public enum JudgeType
    {
        Tap = 0, Hold = 1, Drag = 2, Flick = 3
    }
}

using System.Collections.Generic;
using BM.Data;

namespace BM.Utils
{


    public static class GameplayUtility
    {
        /// <summary> 通过时间和SpeedEvent求出一段Distance </summary>
        public static float CalculateDistance(float time, EachEvent speedEvent)
        {
            // 原理：求梯形面积 Distance = (speedA + speedB) / 2 * (timeB - timeA)

            // 如果SpeedEvent在当前时间之后，就舍去Distance
            if (speedEvent.startTime > time) return 0;

            // 如果SpeedEvent在当前时间之前，就返回完整的Distance
            if (speedEvent.endTime <= time)
                return (speedEvent.endTime - speedEvent.startTime) * (speedEvent.startValue + speedEvent.endValue) * 0.5f;

            // 如果SpeedEvent在当前时间内，就返回插值的Distance
            float realEndValue = GetValueFromTimeAndValue(time, speedEvent.startTime, speedEvent.endTime,
                speedEvent.startValue, speedEvent.endValue, speedEvent.moveType);
            return (float)(time - speedEvent.startTime) * (speedEvent.startValue + realEndValue) * 0.5f;
        }

        /// <summary> 通过时间和首尾数值获得缓动插值 </summary>
        public static float GetValueFromTimeAndValue(float now, float startTime, float endTime, float startValue, float endValue, EaseUtility.Ease ease, float startRange = 0, float endRange = 1)
        {

            if (now < endTime)
            {
                float value = EaseUtility.Evaluate(ease, (float)now - startTime, endTime - startTime, startRange, endRange);
                value = (endValue - startValue) * value + startValue;
                return value;
            }

            return endValue;

        }





        /// <summary> 判断List是否为空 </summary>
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list is not { Count: > 0 };

        public static bool IsNull<T>(this T t) => t == null;

        public static bool IsNotNull<T>(this T t) => t != null;

        public static float GetPositionZFromPosOrder(int order)
        {
            return order * -0.02f;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace BM.Data
{
    [Serializable]
    public class JudgementData
    {
        [FormerlySerializedAs("pERFECTRange")]
        [Header("时间判定/ms")]
        [SerializeField] private int marvelousRange;
        [SerializeField] private int perfectRange;
        [SerializeField] private int goodRange;
        [SerializeField] private int badRange;
        public float MarvelousRange => marvelousRange / 1000f;
        public float PerfectRange => perfectRange / 1000f;
        public float GoodRange => goodRange / 1000f;
        public float BadRange => badRange / 1000f;

        [Header("空间判定/LocalPos")]
        [SerializeField, Tooltip("X上Y下")] private Vector2 heightRange;
        [SerializeField, Tooltip("X左Y右")] private Vector2 widthRange;
        public float UpSideRange => heightRange.x;
        public float DownSideRange => heightRange.y;
        public float LeftSideRange => widthRange.x;
        public float RightSideRange => widthRange.y;
    }
}

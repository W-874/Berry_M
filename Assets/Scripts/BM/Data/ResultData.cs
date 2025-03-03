using System.Collections;
using System.Collections.Generic;
using BM.Data;
using BM.Utils.Singleton;
using UnityEngine;

namespace BM.Data
{
    public class ResultData
    {
        public int Score;
        public float NDK;
        public int PerfectNum
        {
            get
            {
                return Neregol + Ruin;
            }
        }
        public int GreatNum
        {
            get
            {
                return Twist;
            }
        }
        public int BadNum
        {
            get
            {
                return Illusion;
            }
        }
        public int MissNum
        {
            get
            {
                return Reality;
            }
        }
        public int Reality = 0;//miss
        public int Illusion = 0;//bad
        public int Twist = 0;//good
        public int Ruin = 0;// best
        public int Neregol = 0;// master
        public float ND;
        public List<int> judgeList; 

    }
}


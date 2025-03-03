using System;

namespace BM.Gameplay.Managers
{
    public class ScoreManager
    {
        public class ScoreRecord
        {
            public int Index { get; set; }
            public bool IsPlayed { get; set; }
            public int Perfect { get; set; }
            public int Marvelous { get; set; }
            public int Good { get; set; }
            public int Bad { get; set; }
            public int Miss { get; set; }

            public ScoreRecord(int index)
            {
                Index = index;
            }

            public float ToScore()
            {
                throw new NotImplementedException();
            }

            public int ToResultLevel()
            {
                throw new NotImplementedException();
            }

            public float ToAcc()
            {
                throw new NotImplementedException();
            }

            public float ToFixedScore()
            {
                throw new NotImplementedException();
            }

            public int ToCombo(bool b)
            {
                throw new NotImplementedException();
            }
        };
    }
}
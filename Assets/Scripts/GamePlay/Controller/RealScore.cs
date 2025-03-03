using UnityEngine;
using Note;
using System.Collections.Generic;
using Main;
using Map;
using BM.Global;

public class RealScore:MonoBehaviour
{
    public static int RealScoreUpdate(List<int> JudgedDeviation)
    {
        if (JudgedDeviation.Count == 0) return 0;
        int MaxCombo = 0;//连击
        int Score = 0;
        int Combo = 0;
        float Good = NoteController.Good * 1000;
        float Best = NoteController.Best * 1000;
        foreach (var it in JudgedDeviation)
        {
            if (Mathf.Abs(it) <= Good)
            {
                if (Combo++ > MaxCombo)
                    MaxCombo = Combo;
            }
            else
                Combo = 0;
        }
        Combo = 0;
        bool AP = true;
        int PP = 0;
        foreach (var it in JudgedDeviation)
        {
            int t = Mathf.Abs(it);
            if (t <= NoteController.Master * 1000)
            {
                Score += 1000000 / JudgedDeviation.Count;
                Combo++;
            }
            else if (t <= Best)
            {
                Score += 1000000 / JudgedDeviation.Count;
                Combo++;
            }
            else if (t <= Good)
            {
                if (Combo < 20)
                    Score += (int)EasingFunction.Curve(1000000 / JudgedDeviation.Count, 0, t / Good) / 2;
                else
                    Score += (int)EasingFunction.Curve(1000000 / JudgedDeviation.Count, 0, t / Good, EasingType.InQuad) / 3 * 2;
                Combo = 0;
                AP = false;
            }
            else
            {
                Combo = 0;
                AP = false;
            }
        }
        if (AP)
            Score = 1000000;
        Score += (int)EasingFunction.Curve(0, 1000000 - Score, ((float)PP / ((float)JudgedDeviation.Count)));
        return Score;
    }

    private void Start()
    {
        DataContainers.GetRealScore = RealScoreUpdate;
        Destroy(this);
    }
}
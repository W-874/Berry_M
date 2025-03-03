using BM.Data;
using BM.Data.ScriptableObject;
using BM.GameUI.Global;
using BM.GameUI.LevelSelect;
using BM.Utils.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BM.GameUI.Result;
using System.IO;
using BM.Gameplay.Activity;
using Newtonsoft.Json;
using BM.Global;

public class ChartSelectManager : MonoSingleton<ChartSelectManager>
{
    [Header("Asset")] 
    public static LevelData TargetData;
    public static ChapterData ChapterData;
    public static List<ActivityDataObject> ActivityDataObjects;
    public LevelData TargetData__;
    public ChapterData ChapterData__;
    public bool isTest = true;

    public static void StartGame(LevelData data, ChapterData chapter)
    {
        TargetData = data;
        ChapterData = chapter;
        ActivityDataObjects = null;
    }

    public static void StartGame(LevelData data, ChapterData chapter, List<ActivityDataObject> from)
    {
        TargetData = data;
        ChapterData = chapter;
        ActivityDataObjects = from;
    }
    private void Awake()
    {
        if (isTest)
        {
            TargetData = TargetData__;
            ChapterData = ChapterData__;
        }
        else
        {
            TargetData__ = TargetData;
            if (ChapterData == null)
            {
                ChapterData = ChapterData__;
            }else ChapterData__ = ChapterData;
        }
    }

    public static void ReturnBack()
    {
        LevelSelectManager.Init(ChapterData);
        TransitionManager.DoScene("Scenes/LevelSelectionScene", Color.black, 0.25f, 0.5f, 0.5f);
    }

    public static void RePlay()
    {
        StartGame(TargetData, ChapterData);
        TransitionManager.DoScene("Scenes/GameplayScene", Color.black, 0.25f, 0.5f, 0.5f);
    }

    public static void EndGame(ResultData data, float ndv,int score)
    {
        bool isnewbest = false;
        if (TargetData.songName != "新手教程" && score > DataContainers.Get_RealScore(TargetData.songName, TargetData.TargetDiff))
            isnewbest = true;
        if (TargetData.songName != "新手教程")
        {
            ResultManager.Init(data, TargetData, score, isnewbest);
            TransitionManager.DoScene("Scenes/ResultScene", Color.black, 0.25f, 0.5f, 0.5f);
        }
        else
        {
            TransitionManager.DoScene("Scenes/ChapterSelectScene", Color.black, 0.25f, 0.5f, 0.5f);
        }

    }
}

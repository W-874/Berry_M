using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UIController;
using Main;
using Base;
using UnityEngine.SceneManagement;
using Map;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Runtime.InteropServices;
using System.IO;
using System;
using Track;
using BM;
using BM.Data;
using BM.Data.ScriptableObject;
using BM.Gameplay.Activity;
using Note;
using BM.Global;
using Message;
using UnityEngine.Serialization;

public class MainElementController : UnderlyingObject
{
    [Header("Left")]
    public TMP_Text SongName;
    public SpriteController SCI;
    [Header("Right")]
    public TMP_Text Score;
    public TMP_Text Composer;
    public TMP_Text NDdiff;
    [Header("Combo")]
    public TMP_Text Combo;
    public SpriteController SCBG;
    [Header("Others")]
    public NDVBroad InitNDVBraod;

    public ActivityCanvaController activityCanvaController;

    float Tc = 1;
    float Tc2 = 1;
    GlobalSettings global;

    private void Start()
    {
        Application.targetFrameRate = 60;

        SetGlobalSettings();

        if (ChartSelectManager.TargetData == null)
            SceneManager.LoadSceneAsync(0);
        MainCommander.Main.UIoutput = UIoutput;

        Add(EndAndInitNDV);
        StartCoroutine(nameof(ReadyStart));


    }

    void SetGlobalSettings()
    {
        global = GlobalSettings.CurrentSettings;

        //NoteController.Mask = EasingFunction.Curve(0, 1, global.Mask, EasingType.OutQuart);
        float width = EasingFunction.Curve(0, 20, global.Mask);
        MainCommander.Main.startVec.transform.localScale = new Vector3(width, width, 1);

        Add((Carrier carrier) =>
        {
            StopSPC.MainSPC.volume = SourcePlayer.SourcePlayer.source.volume = global.MusicVolume;
            carrier.state = State.Destroy;
        });

        float cus = global.Lighting;
        SCBG.SetColor(1 * cus, (0.7f + Tc * 0.3f) * cus, (0.7f + Tc * 0.3f) * cus);
        SCBG.SetColor((0.7f + Tc2 * 0.3f) * cus, (0.7f + Tc2 * 0.3f) * cus, 1 * cus);

        Add((Carrier) =>
        {
            float l = global.Lighting;
            if (Tc < 1)
            {
                Tc += 0.05f;
                SCBG.SetColor(1 * l, (0.7f + Tc * 0.3f) * l, (0.7f + Tc * 0.3f) * l);
            }
            if (Tc2 < 1)
            {
                Tc2 += 0.05f;
                SCBG.SetColor((0.7f + Tc2 * 0.3f) * l, (0.7f + Tc2 * 0.3f) * l, 1 * l);
            }
        });

        float l = global.Lighting;
        SCBG.SetColor(1 * l, (0.7f + Tc * 0.3f) * l, (0.7f + Tc * 0.3f) * l);
        SCBG.SetColor((0.7f + Tc2 * 0.3f) * l, (0.7f + Tc2 * 0.3f) * l, 1 * l);

        MainCommander.Global.GlobalOffset = -global.MusicOffset / 1000.0f;

        NoteController.Speed = global.GlobalNoteSpeed;
        NoteController.HitFxSize = global.HitFxSize;
        NoteController.HitSoundVolume = global.HitSoundVolume ;

        if ((global.IsTest & (1 << 0)) == 1) 
            StopSPC.TargetState = TouchController.MainTouch.TS = TouchController.TouchState.InTest;
        if ((global.IsTest & (1 << 1)) == 1)
            NoteController.Best = NoteController.Good;
        if ((global.IsTest & (1 << 2)) == 1)
            NoteController.Bad = NoteController.Good;
        if ((global.IsTest & (1 << 3)) == 1)
        {
            NoteController.Bad = global.JudgeLine;
            NoteController.Good = global.JudgeLine - 0.010f;
            NoteController.Best = global.JudgeLine - 0.040f;
            NoteController.Master = global.JudgeLine - 0.100f;

            TouchController.JudgeAngle = global.JudgeAngle;
        }

    }

    IEnumerator ReadyStart()
    {
        if (ChartSelectManager.TargetData == null)
        {
            SceneManager.LoadSceneAsync("Scene/MainScene");
            yield break;
        }
        MainCommander.Map = new BmTbm(new(), new());
        var data = JsonConvert.DeserializeObject(
            Resources.Load<TextAsset>(ChartSelectManager.TargetData.GetPath()).text,
            new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Populate
            });
        JsonUtility.FromJsonOverwrite(data.ToString(), MainCommander.Map);

        if (ChartSelectManager.ActivityDataObjects != null)
        {
            activityCanvaController.Init(ChartSelectManager.ActivityDataObjects);
        }
        
        //JsonUtility.FromJsonOverwrite(Resources.Load<TextAsset>(ChartSelectManager.TargetData.GetPath()).text, MainCommander.Map);
        MainCommander.Main.themeController.currentJudgement 
            = Resources.Load<GameObject>("Note/" + MainCommander.Map.MainBm.JudgementTheme.ToString());
        SetIllustration(ChartSelectManager.TargetData.Illustration);
        SetSongName(ChartSelectManager.TargetData.songName);
        SetComposer(ChartSelectManager.TargetData.songArtist);
        SetDiff(ChartSelectManager.TargetData.TargetDiff, (int)MainCommander.Map.MainBm.NeregolDreamLevel);
        float currenttime = Time.time;
        while(Time.time-currenttime<=3)
        {
            MainCommander.Global.GlobalOffset = (Time.time - currenttime)-3-global.MusicOffset / 1000.0f;
            yield return new();
        }
        MainCommander.Global.GlobalOffset = -global.MusicOffset / 1000.0f;
        SourcePlayer.SourcePlayer.Clip = ChartSelectManager.TargetData.GetSongClip();
        SourcePlayer.SourcePlayer.source.volume = GlobalSettings.CurrentSettings.MusicVolume;
        SourcePlayer.SourcePlayer.source.Play();
    }

    void UIoutput(int deviation,bool isWarning,bool isGoodWaring, int combo,int score)
    {
        SetCombo(combo, isWarning, isGoodWaring);
        SetScore(score);
    }

    public void EndAndInitNDV(Carrier carrier)
    {
        if(MainCommander.Main.IsGameEnd)
        {
            if (global.IsTest != 0) ChartSelectManager.ReturnBack();
            InitNDVBraod.Init();
            MainCommander.Main.IsGameEnd = false;
            StartCoroutine(nameof(ToResult));
        }
    }

    IEnumerator ToResult()
    {
        ResultData data = new();
        foreach (var it in MainCommander.JudgedDeviation)
        {
            if (Mathf.Abs(it) <= NoteController.Master * 1000) data.Neregol++;
            else if (Mathf.Abs(it) <= NoteController.Best * 1000) data.Ruin++;
            else if (Mathf.Abs(it) <= NoteController.Good * 1000) data.Twist++;
            else if (Mathf.Abs(it) <= NoteController.Bad * 1000) data.Illusion++;
            else data.Reality++;
        }
        data.Score = MainCommander.Main.Score;
        data.judgeList = MainCommander.JudgedDeviation;
        data.ND = MainCommander.Main.NeregolDreamValue;
        yield return new WaitForSeconds(3);
        ChartSelectManager.EndGame(data, MainCommander.Main.NeregolDreamValue,MainCommander.Main.Score);
    }

    public void SetDiff(NeregolLevel diff,int diffnum)
    {
        NDdiff.text = diff.ToString() + "/" + diffnum.ToString() + ((diffnum - (int)diffnum > 0.5f) ? "+" : "");
    }

    public void SetComposer(string Composername)
    {
        Composer.text = Composername;
    }

    public void SetIllustration(Sprite sprite)
    {
        SCI.UpdateSprite(sprite);
    }

    public void SetSongName(string name)
    {
        SongName.text = name;
    }

    public void SetScore(int score)
    {
        Score.text = score.ToString();
    }

    public void SetCombo(int combo,bool isWaring,bool isGoodWaring)
    {
        Combo.text = combo.ToString();
        if (isWaring)
            Tc = 0;
        if (isGoodWaring)
            Tc2 = 0;
    }

    public void OnEnd()
    {
        gameObject.SetActive(false);
    }

   
}

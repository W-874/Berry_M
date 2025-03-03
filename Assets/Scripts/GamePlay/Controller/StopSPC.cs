using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;
using UIController;
using Message;
using SourcePlayer;
using TMPro;
using UnityEngine.UI;
using Main;
using BM.Data;
using BM.Global;

public class StopSPC : UnderlyingObject
{
    public static StopSPC MainSPC;
    public static TouchController.TouchState TargetState = TouchController.TouchState.InChart;
    [Header("StopBroad")]
    public RectTransform StopBroad;
    // [Header("DataObject")]
    // public TMP_Text SongName;
    // public TMP_Text Charter;
    // public TMP_Text Composer;
    // public TMP_Text Score;
    // public TMP_Text Tips;
    // public TMP_Text BestScore;
    [Header("Data")]
    public float volume;
    [Header("Button")]
    public Button ConPlayButton;
    public Button RePlayButton;
    public Button BackButton;

    int ConPlay = 0;

    private void Start()
    {
        MainSPC = this;
        GetComponent<Button>().onClick.AddListener(StopClick);
        ConPlayButton.onClick.AddListener(PlayClick);
        RePlayButton.onClick.AddListener(RePlayClick);
        BackButton.onClick.AddListener(BackClick);
        StopBroad.gameObject.SetActive(false);
    }

    public void StopClick()
    {
        if (ChartSelectManager.ChapterData.identifier == "NULL") return;
        SourcePlayer.SourcePlayer.source.Pause();
        TouchController.MainTouch.TS = TouchController.TouchState.Default;
        StopBroad.gameObject.SetActive(true);
        ConPlay = 0;
        // SongName.text = ChartSelectManager.TargetData.songName;
        // Charter.text = ChartSelectManager.TargetData.chartArtist;
        // Composer.text = ChartSelectManager.TargetData.songArtist;
        // Score.text = MainCommander.Main.Score.ToString();
        // Tips.text = "ª∂”≠¿¥µΩ≤‚ ‘";
        // BestScore.text 
        //     = DataContainers.Get_RealScore(ChartSelectManager.TargetData.songName, ChartSelectManager.TargetData.TargetDiff).ToString();
    }

    public void PlayClick()
    {
        SourcePlayer.SourcePlayer.source.Play();
            SourcePlayer.SourcePlayer.source.pitch = -0.1f;
        SourcePlayer.SourcePlayer.source.volume = 0;
        TouchController.MainTouch.Clear();
        StartCoroutine(ConPlay_(Time.time));
    }

    private IEnumerator ConPlay_(float Value)
    {
        StopBroad.gameObject.SetActive(false);
        while (Time.time - Value < 3&&SourcePlayer.SourcePlayer.source.time >0.1f) yield return null;
        SourcePlayer.SourcePlayer.source.pitch = 1;
        SourcePlayer.SourcePlayer.source.volume = volume;
        TouchController.MainTouch.TS = TouchController.TouchState.InChart;
    }

    public void RePlayClick()
    {
        ChartSelectManager.RePlay();
    }

    public void BackClick()
    {
        ChartSelectManager.ReturnBack();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            StopClick();
        }
    }
}

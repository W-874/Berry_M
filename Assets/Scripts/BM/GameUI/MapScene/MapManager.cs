using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BM.Data;
using BM.Data.ScriptableObject;
using BM.GameUI.Global;
using BM.GameUI.LevelSelect;
using BM.Global;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoSingleton<MapManager>
{
    // Start is called before the first frame update
    [SerializeField] private Button horizonRamblue, lightToHeed;
    [SerializeField] private Button backButton, settingButton;

    [SerializeField] private LevelDataObject[] horizonBlue, lightTo;

    [SerializeField] private ChapterDataObject[] schapterDataObjects;

    private ChapterData horizon, light;
    private ChapterData _chapterData;

    protected override void OnAwake()
    {
        backButton.onClick.AddListener(() => TransitionManager.DoScene(SceneToDo, Color.white, 0.25f));
        settingButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/SettingsScene", Color.white, 0.25f));
        
        _chapterData = schapterDataObjects.Select(x => x.CurrentData).ToArray()[0];

        horizon = new ChapterData(_chapterData.identifier, _chapterData.illustrationID, _chapterData.chapterName,
            _chapterData.chapterIntroduction, horizonBlue);
        light = new ChapterData(_chapterData.identifier, _chapterData.illustrationID, _chapterData.chapterName,
            _chapterData.chapterIntroduction, lightTo);
        
        horizonRamblue.onClick.AddListener(() => IntoHorizonRamblue());
        lightToHeed.onClick.AddListener(() => IntoLightInHeed());
        
        LevelSelectManager.SetSceneToDo("Scenes/MapScene");
    }
    
    public void IntoHorizonRamblue()
    {
        LevelSelectManager.Init(horizon);
        TransitionManager.DoScene("Scenes/LevelSelectionScene", Color.white, 0.25f);
    }

    public void IntoLightInHeed()
    {
        LevelSelectManager.Init(light);
        TransitionManager.DoScene("Scenes/LevelSelectionScene", Color.white, 0.25f);
    }
}

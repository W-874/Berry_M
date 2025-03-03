using System;
using System.Collections.Generic;
using System.Linq;
using BM.Data;
using BM.Data.ScriptableObject;
using BM.Gameplay.Activity;
using UnityEngine;
using BM.Utils.Singleton;
using UnityEngine.UI;
using BM.GameUI.Global;
//using BM.Gameplay.Managers;
using BM.GameUI.ChapterSelect;
using BM.GameUI.Character;
using BM.GameUI.Entry;
using BM.GameUI.Result;
using BM.GameUI.Settings;
using BM.Global;

namespace BM.GameUI.Main
{
    public class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private Button settingButton, chapterSelectButton, characterSelectButton;
        [SerializeField] private Image Partner;
        [SerializeField] private GameObject prefab;

        private static CharaData[] charaDatas;

        public ChapterData ChapterData;
        [SerializeField] private LevelDataObject[] levelDatasObjects;
        [SerializeField] private List<ActivityDataObject> activityDataObjects;


        protected override void OnAwake()
        {
            settingButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/SettingsScene", Color.white, 0.25f));
            chapterSelectButton.onClick.AddListener(IntoNew);
            characterSelectButton.onClick.AddListener(() => TransitionManager.DoScene("Scenes/CharaSelectScene", Color.white, 0.25f));

            ChapterData.levelData = levelDatasObjects.Select(x => x.CurrentData).ToArray();
            
            Partner.sprite = charaDatas[DataContainers.GetPartner()]._Sprite;
            ResultManager.InitPartner(charaDatas[DataContainers.GetPartner()]);
            
            ChapterSelectManager.SetSceneToDo("Scenes/MainScene");
            SettingsManager.SetSceneToDo("Scenes/MainScene");
            CharacterSelectManager.SetSceneToDo("Scenes/MainScene");
        }

        public static void Init(CharaDataObject[] charaDataObjects)
        {
            charaDatas = charaDataObjects.Select(x => x.CurrentData).ToArray();

        }

        private void IntoNew()
        {
            GlobalSettings globalSettings = GlobalSettings.CurrentSettings;
            LevelData levelData = ChapterData.levelData[0];
            if (globalSettings.FirstPlay)
            //if (true)
            {
                levelData.TargetDiff = NeregolLevel.Reality;
                ChartSelectManager.StartGame(levelData, null, activityDataObjects);
                TransitionManager.DoScene("Scenes/GameplayScene", Color.black, 0.25f, 0.5f, 0.5f);
                globalSettings.FirstPlay = false;
                GlobalSettings.CurrentSettings = globalSettings;
            }
            else
            {
                TransitionManager.DoScene("Scenes/ChapterSelectScene", Color.white, 0.25f);
            }
        }
    }
}
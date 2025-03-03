using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BM.Data;
using BM.Data.ScriptableObject;
using BM.GameUI.CharaShow;
using BM.GameUI.Global;
using BM.Utils;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BM.GameUI.Character
{
    public class CharacterSelectManager : MonoSingleton<CharacterSelectManager>
    {
        [SerializeField] private CharaShowerControl charaPrefab;
        [SerializeField] private GameObject content;
        [SerializeField] private Button backButton;
        [SerializeField] private CharaDataObject[] CharaDataObjects;

        public static CharaData[] CharaDatas;
        public List<CharaShowerControl> CharacterShowers { get; private set; } = new();


        protected override void OnAwake()
        {
            backButton.onClick.AddListener(() => TransitionManager.DoScene(SceneToDo, Color.white, 0.25f));
            
            CharaDatas = CharaDataObjects.Select(x => x.CurrentData).ToArray();
            
            if (CharaDatas.IsNullOrEmpty()) return;
            for (var i = 0; i < CharaDatas.Length; i++)
            {
                var obj = Instantiate(charaPrefab.gameObject,content.transform);
                var comp = obj.GetComponent<CharaShowerControl>();
                comp.Init(CharaDatas[i]);
                CharacterShowers.Add(comp);
            }
            
            CharaShowManager.SetSceneToDo("Scenes/CharaSelectScene");

        }

        public static void IntoShow(int index, CharaData[] charaData)
        {
            CharaShowManager._init(charaData, index - 1);
            TransitionManager.DoScene("Scenes/CharaShowScene", Color.white, 0.25f);
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using BM.Data;
using BM.GameUI.Global;
using BM.Global;
using BM.Utils;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BM.GameUI.CharaShow
{
    public class CharaShowManager : MonoSingleton<CharaShowManager>
    {
        [SerializeField] private CharaShower showerPrefab;
        [SerializeField] private Button upShower, downShower, applyButton;
        [SerializeField] private Transform showerTransform;
        [SerializeField] private Image upImage, downImage;
        [SerializeField] private Button backButton;

        private static CharaData[] charaDatas;

        private List<CharaShower> _charaShowers { get; set; } = new();

        private static int nowIndex;

        private int NowIndex { get; set; }


        public static void _init(CharaData[] charaData, int index)
        {
            charaDatas = charaData;
            nowIndex = index;
        }

        protected override void OnAwake()
        {
            NowIndex = nowIndex;
            backButton.onClick.AddListener(() => TransitionManager.DoScene(SceneToDo, Color.white, 0.25f));
            
            if (charaDatas.IsNullOrEmpty())
            {
                SceneManager.LoadSceneAsync("Scenes/CharaSelectScene");
                return;
            }
            
            upShower.onClick.AddListener(() => UpdateCharaShower(-1));
            downShower.onClick.AddListener(() => UpdateCharaShower(1));
            applyButton.onClick.AddListener(() => DataContainers.SetPartner(NowIndex));
        }

        private void Start()
        {
            for (int i = 0; i < charaDatas.Length; i++)
            {
                var charaData = charaDatas[i];

                CharaShower shower = Instantiate(showerPrefab.gameObject, showerTransform).GetComponent<CharaShower>();
                shower.Init_Shower(charaData);
                _charaShowers.Add(shower);

                if (i != NowIndex)
                {
                    shower.gameObject.SetActive(false);
                }
            }

            upImage.sprite = charaDatas[IndexR(NowIndex - 1, charaDatas)]._Sprite;
            downImage.sprite = charaDatas[IndexR(NowIndex + 1, charaDatas)]._Sprite;

        }
        

        private void UpdateCharaShower(int type)
        {
            _charaShowers[NowIndex].gameObject.SetActive(false);
            NowIndex = IndexR((NowIndex + type), charaDatas);
            _charaShowers[NowIndex].gameObject.SetActive(true);
            
            upImage.sprite = charaDatas[IndexR(NowIndex - 1,charaDatas)]._Sprite;
            downImage.sprite = charaDatas[IndexR(NowIndex + 1, charaDatas)]._Sprite;
        }
        
        private static int IndexR(int index, CharaData[] data)
        {
            int len = data.Length;

            if (index > len - 1)
            {
                index = 0;
            }else if (index < 0)
            {
                index = len - 1;
            }

            return index;

        }
        
        
    }

}

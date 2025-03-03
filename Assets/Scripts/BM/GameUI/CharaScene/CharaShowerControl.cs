using System;
using BM.Data;
using BM.GameUI.Global;
using UnityEngine;
using UnityEngine.UI;

namespace BM.GameUI.Character
{
    public class CharaShowerControl : MonoBehaviour
    {
        [SerializeField] private Image illusion, nameBox;
        [SerializeField] private Text name;
        [SerializeField] private Button intoCharaShow;

        private int _index;
        public void Init(CharaData charaData)
        {
            illusion.sprite = charaData._Sprite;
            nameBox.color = charaData.NameBoxColor;
            name.text = charaData.charaName;
            _index = charaData.identifier;
        }

        public void Awake()
        {
            intoCharaShow.onClick.AddListener(() => IntoCharaShow(_index, CharacterSelectManager.CharaDatas));
        }

        private void IntoCharaShow(int index, CharaData[] charaData)
        {
            CharacterSelectManager.IntoShow(index, charaData);
        }
    }

}

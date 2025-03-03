using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BM.Data.ScriptableObject;

namespace BM.Data
{
    [Serializable]
    public class CharaData
    {
        public int identifier;
        public Sprite _Sprite;
        public string charaName;
        public string charaIntroduction;
        public Color NameBoxColor;

        public CharaData(int id, Sprite illustration, string name, string introduction, Color color)
        {
            identifier = id;
            _Sprite = illustration;
            charaName = name;
            charaIntroduction = introduction;
            NameBoxColor = color;
        }
    }
}


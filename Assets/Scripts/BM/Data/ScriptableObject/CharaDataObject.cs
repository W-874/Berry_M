using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BM.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "PDR/Character Data", fileName = "New Charater Data")]
    public class CharaDataObject : UnityEngine.ScriptableObject
    {
        [SerializeField, Header("角色ID")] private int identifier;
        [SerializeField, Header("角色立绘")] private Sprite illustration;
        [SerializeField, Header("角色名字")] private string chapterName;
        [SerializeField, Header("角色描述")] private string chapterTitle;
        [SerializeField, Header("名字框颜色")] private Color nameBoxColor;
        public CharaData CurrentData => new(identifier, illustration, chapterName, chapterTitle, nameBoxColor);
    }
}


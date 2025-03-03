using System.Collections;
using System.Collections.Generic;
using BM.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CharaShower : MonoBehaviour
{
    [SerializeField] private Image illusion, bgIllusion;
    [SerializeField] private Text name, intro;

    public void Init_Shower(CharaData charaData)
    {
        illusion.sprite = charaData._Sprite;
        bgIllusion.sprite = charaData._Sprite;
        name.text = charaData.charaName;
        intro.text = charaData.charaIntroduction;

        intro.text = intro.text.Replace("\\n", "\n");
        intro.text = intro.text.Replace(" ", "\u3000");
    }


}

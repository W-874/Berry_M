using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;
using TMPro;
using Main;

public class NDVBroad : UnderlyingObject
{
    public TMP_Text NDVnum;

    public void Init()
    {
        GetComponent<Animator>().SetBool("IsInit", true);
        NDVnum.text = MainCommander.Main.NeregolDreamValue.ToString();
    }

    public void EndInit()
    {
        GetComponent<Animator>().SetBool("IsInit", false);
    }
}

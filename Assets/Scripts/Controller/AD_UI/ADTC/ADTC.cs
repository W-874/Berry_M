using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.InteropServices;

namespace Item.Control.AD
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class ADTC : Item
    {
        public override void Init(Item_Map from) { }

        public override bool IsIgnore() { return true; }

        [SerializeField] TMP_Text text;
        [SerializeField] string BehindEnder;

        public override void Awake()
        {
            base.Awake();
            text.text = BehindEnder;
        }

        public void SetText(string from)
        {
            text.text = from;
        }
    }
}

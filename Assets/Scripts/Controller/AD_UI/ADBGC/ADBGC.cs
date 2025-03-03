using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Item.UI
{
    public class ADBGC : Item
    {
        public List<ADBG_ChildC> childs;

        public void SetActive(bool t)
        {
            foreach (var it in childs) it.SetActive(t);
        }

        public override bool IsIgnore()
        {
            return true;
        }

        public override void Init(Item_Map from) { }
    }
}

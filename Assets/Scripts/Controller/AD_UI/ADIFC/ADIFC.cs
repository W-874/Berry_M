using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Base;
using System;
using System.Runtime.InteropServices;
using TMPro;

namespace Item.UI
{
    public class ADIFC :MonoBehaviour
    {
        public TMP_Text Target;

        TMP_InputField m_InputField;
        TMP_InputField inputField
        {
            get
            {
                if (m_InputField == null)
                    m_InputField = GetComponent<TMP_InputField>();
                return m_InputField;
            }
        }

        public void OnChange()
        {
            Target.text = inputField.text;
        }

        public void OnEnd()
        {
            Target.text = inputField.text;
            gameObject.SetActive(false);
        }
    }
}

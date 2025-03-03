using System.Collections;
using System.Collections.Generic;
using BM.GameUI.Result;
using UnityEngine;
using UnityEngine.UI;

namespace BM.Global
{
    public class PlayerDataController : MonoBehaviour
    {
        [SerializeField] Text Name;
        [SerializeField] Text Lv;
        [SerializeField] AudioSource ad;

        // Start is called before the first frame update
        void Start()
        {
            GlobalSettings global = GlobalSettings.CurrentSettings;
            Name.text = global.PlayerName;
            if (ResultManager.CharaData != null)
                if (ResultManager.CharaData.charaName != "新手教程")
                    Lv.text = "NDV" + DataContainers.GetNeregolDreamValue().ToString("F2");
                else
                    Lv.text = "NDV" + DataContainers.GetNeregolDreamValue().ToString("F2");
            ad.volume = global.MainVolume;
        }
    }
}

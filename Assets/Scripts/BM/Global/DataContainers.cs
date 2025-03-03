using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BM;
using BM.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace BM.Global
{
    public class DataContainers : MonoBehaviour
    {
        public static DataContainers main;

        public delegate int RealScore(List<int> judges);
        public static RealScore GetRealScore;

        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class Data
        {
            public Dictionary<string, Dictionary<NeregolLevel, List<int>>> Charts = new();
            public Dictionary<string, (NeregolLevel, float)> Best20NDV = new();

            public Data()
            {
                Charts = new();
                Best20NDV = new();
            }
        }

        static Data GloablData = new();
        public static Dictionary<string, Dictionary<NeregolLevel, List<int>>> Charts
        {
            get
            {
                return GloablData.Charts;
            }
            set
            {
                GloablData.Charts = value;
            }
        }
        public static Dictionary<string, (NeregolLevel, float)> Best20NDV 
        {
            get
            {
                return GloablData.Best20NDV;
            }
            set
            {
                GloablData.Best20NDV = value;
            }
        }
        public static Action UIoutput;

        public static float GetNeregolDreamValue()
        {
            float targetbestndv = 0;
            foreach(var it in Best20NDV)
            {
                targetbestndv += it.Value.Item2 / 20.0f;
            }
            return targetbestndv;
        }

        public static void UpdataBest(string songname, NeregolLevel level,float ND)
        {
            float lownbv = 10000;
            string lowsong = "";
            foreach(var it in Best20NDV)
            {
                if (it.Value.Item2 < lownbv)
                    lowsong = it.Key;
            }
            if (Best20NDV.Count < 20)
            {
                if (lowsong == songname)
                    Best20NDV[songname] = (level, ND);
                else
                    Best20NDV.TryAdd(songname, (level, ND));
            }
            else if (Best20NDV[lowsong].Item2 < ND)
            {
                Best20NDV.Remove(lowsong);
                Best20NDV.TryAdd(songname, (level, ND));
            }
        }

        public static int Get_RealScore(string song,NeregolLevel level)
        {
            List<int> cat = GetData(song, level);
            return GetRealScore(cat);
        }

        public static List<int> GetData(string songname, NeregolLevel diff)
        {
            return TryUpdata(songname, diff, new());
        }

        static List<int> TryUpdata(string songname, NeregolLevel diff, List<int> inout)
        {
            Charts.TryAdd(songname, new Dictionary<NeregolLevel, List<int>>());
            Charts[songname].TryAdd(diff, new());
            if (inout.Count == 0)
                return inout = Charts[songname][diff];
            else
                return Charts[songname][diff] = inout;
        }

        public static void Updata(string songname, NeregolLevel level, List<int> newJudges)
        {
            TryUpdata(songname, level, newJudges);
        }

        public static void Save(string song, NeregolLevel level)
        {
            Save_(song,level);
        }

        static void Save_(string song,NeregolLevel level)
        {
            string path;
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                path = Path.Combine(Application.persistentDataPath, "data.json");
            else
                path = Path.Combine(Application.streamingAssetsPath, "data.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(GloablData));

            UIoutput?.Invoke();
        }

        public static void TryCreateDirectroryAndFile(string filePath)
        {
            Debug.Log($"CreateDirectrory {filePath}[folder_path],");
            if (!string.IsNullOrEmpty(filePath))
            {
                string dir_name = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir_name))
                {
                    Debug.Log($"No Exists {dir_name}[dir_name],");
                    Directory.CreateDirectory(dir_name);
                }
                else
                {
                    Debug.Log($"Exists {dir_name}[dir_name],");
                }
            }
        }

        public static int GetHistoryChapter()
        {
            return GlobalSettings.CurrentSettings.HistoryChapter;
        }
        public static int GetHistoryChart()
        {
            return GlobalSettings.CurrentSettings.HistoryChart;
        }

        public static NeregolLevel GetHistoryDiff()
        {
            return GlobalSettings.CurrentSettings.HistoryDiff;
        }

        public static int GetPartner()
        {
            return GlobalSettings.CurrentSettings.Partner;
        }
        
        public static void SetHistoryChapter(int from)
        {
            SetHistoryChart(0);
            var cat = GlobalSettings.CurrentSettings;
            cat.HistoryChapter = from;
            GlobalSettings.CurrentSettings = cat;
        }
        public static void SetHistoryChart(int from)
        {
            var cat = GlobalSettings.CurrentSettings;
            cat.HistoryChart = from;
            GlobalSettings.CurrentSettings = cat;
        }
        public static void SetHistoryDiff(NeregolLevel from)
        {
            var cat = GlobalSettings.CurrentSettings;
            cat.HistoryDiff = from;
            GlobalSettings.CurrentSettings = cat;
        }

        public static void SetPartner(int charaData)
        {
            var cat = GlobalSettings.CurrentSettings;
            cat.Partner = charaData;
            GlobalSettings.CurrentSettings = cat;
        }
        

        private void Start()
        {
            main = this;
            DontDestroyOnLoad(gameObject);
            string path;
            if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
               path = Path.Combine(Application.persistentDataPath, "data.json");
            else
                path = Path.Combine(Application.streamingAssetsPath, "data.json");
            if (!File.Exists(path))
            {
                TryCreateDirectroryAndFile(path);

                GloablData = new();
                File.WriteAllText(path, JsonConvert.SerializeObject(GloablData));
            }
            else
            {
                GloablData = JsonConvert.DeserializeObject<Data>(File.ReadAllText(path));
            }
        }
    }
}
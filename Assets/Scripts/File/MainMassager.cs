using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace Main
{
    [Serializable]
    public class MainMassager
    {
        [Serializable] public class SaveEvent : UnityEvent<string> { }
        [Serializable] public class LoadEvent : UnityEvent<string> { }

        public SaveEvent OnSave;
        public LoadEvent OnLoad;
        string ObjectName;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
        public string path
        {
            get
            {
                string path_;
                if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                    path_ = Path.Combine(Application.persistentDataPath, ObjectName);
                else
                    path_ = Path.Combine(Application.streamingAssetsPath, ObjectName);
                FileC.FileC.TryCreateDirectroryOfFile(path_);
                Debug.Log(path_);
                return path_;
            }
            set
            {
                ObjectName = value;
            }
        }

        //两个函数顾名思义
        public void Save()
        {
            OnSave?.Invoke(path);
        }
        public void Load()
        {
            OnLoad?.Invoke(path);
        }
    }

}

namespace FileC
{
    public static class FileC
    {
        //生成这个文件的文件路径（不包括本身）
        public static void TryCreateDirectroryOfFile(string filePath)
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
    }
}
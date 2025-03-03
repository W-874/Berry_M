using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;
using Main.Control;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Serialization;

namespace Main
{
    public class MainSystem : MonoBehaviour
    {
        public static MainSystem main;

        [Header("CurrentAD_Project")] public string TargetAD = "Main";

        [FormerlySerializedAs("massager")] [Header("Massager")]
        public MainMassager messenger = new();

        public bool ADFunction = true;
        [Header("Prefab")] public Dictionary<Item.Item, GameObject> Prefabs;

        private void Awake()
        {
            if (main != null)
            {
                Destroy(gameObject);
            }
            else
                main = this;
        }

        private void Start()
        {
            if (ADFunction) DontDestroyOnLoad(gameObject);
            Init();
        }

        //被要求进入下一个AD时准备跳转并重置目标名
        public void Init(string NewTarget)
        {
            TargetAD = NewTarget;
            StartCoroutine(InitScene());
        }

        //生成时进行的初始化
        private void Init()
        {
            if (!ADFunction) return; //非AD图的保存模式将禁用这个默认的初始化
            messenger.path = TargetAD;

            messenger.OnSave.RemoveAllListeners();
            messenger.OnLoad.RemoveAllListeners();

            //messenger.OnSave.AddListener(BmoSaveLoad.Save);
            //messenger.OnLoad.AddListener(BmoSaveLoad.Load);
            if (PMovement.main != null)
            {
                messenger.OnSave.AddListener(PMovement.main.OnSave);
                messenger.OnLoad.AddListener(PMovement.main.OnLoad);
            }

            messenger.Load();
        }

        IEnumerator InitScene()
        {
            var operation = SceneManager.LoadSceneAsync(0);
            while (!operation.isDone) yield return null;
            Init();
        }

        public void Save()
        {
            messenger.Save();
        }

        public void DEBUGTEST()
        {
            Debug.LogWarning("DEBUGTEST");
        }

        public void DEBUGTEST(float from)
        {
            Debug.LogWarning("DEBUGTEST");
        }

        public void DEBUGTEST(int from)
        {
            Debug.LogWarning("DEBUGTEST");
        }

        public void DEBUGTEST(bool from)
        {
            Debug.LogWarning("DEBUGTEST");
        }
    }
}
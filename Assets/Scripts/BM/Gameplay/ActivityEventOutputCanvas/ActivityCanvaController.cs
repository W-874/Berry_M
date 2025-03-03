using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BM.Data.ScriptableObject;

namespace BM.Gameplay.Activity
{
    public class ActivityCanvaController : MonoBehaviour
    {
        [Header("Asset")]
        public List<ActivityDataObject> ActivityDataObjects = new();
        public List<GameObject> ActivityObjects = new();
        public int index = 0;
        public int endindex = 0;
        public GameObject Prefab;
        
        public void Init(List<ActivityDataObject> from)
        {
            ActivityDataObjects = from;
            ActivityDataObjects.Sort((T, S) => T.Greater(S));
            foreach(var it in ActivityDataObjects)
            {
                var cat = Instantiate(Prefab, transform);
                it.Init(cat);
                ActivityObjects.Add(cat);
                ActivityDataObject.UpdataObjA(cat, 0);
            }
        }

        private void Init()
        {
            ActivityDataObjects.Sort((T, S) => T.Greater(S));
            foreach (var it in ActivityDataObjects)
            {
                var cat = Instantiate(Prefab, transform);
                it.Init(cat);
                ActivityObjects.Add(cat);
                ActivityDataObject.UpdataObjA(cat, 0);
            }
        }

        private void Update()
        {
            if (ActivityObjects.Count != ActivityDataObjects.Count) Init();
            index = -1;
            while (++index < ActivityDataObjects.Count && !ActivityDataObjects[index].isActivity(Main.MainCommander.TimeWithOffset)) ;
            endindex = index;
            while (endindex < ActivityDataObjects.Count && ActivityDataObjects[endindex++].isActivity(Main.MainCommander.TimeWithOffset)) ;
            for (int i = index; i < endindex; i++)
                ActivityDataObject.UpdataObjA(ActivityObjects[i], ActivityDataObjects[i].CurrentValue(Main.MainCommander.TimeWithOffset));
        }
    }
}
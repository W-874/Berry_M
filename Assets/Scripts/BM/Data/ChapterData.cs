using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BM.Data.ScriptableObject;

namespace BM.Data
{
    [Serializable]
    public class ChapterData
    {
        public string identifier;
        public Sprite illustrationID;
        public string chapterName;
        public string chapterIntroduction;
        public LevelData[] levelData;

        public ChapterData(string id, Sprite illustration, string name, string title, IEnumerable<LevelDataObject> levelDataObjects)
        {
            identifier = id;
            illustrationID = illustration;
            chapterName = name;
            chapterIntroduction = title;
            levelData = levelDataObjects.Select(x =>
            {
                var data = x.CurrentData;
                data.PathFather = x.PathFather;
                return data;
            }).ToArray();
        }
    }
}

using UnityEngine;

namespace BM.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "PDR/Level Data", fileName = "New Level Data")]
    public class LevelDataObject : UnityEngine.ScriptableObject
    {
        [SerializeField, Header("关卡数据")] private LevelData levelData;
        [SerializeField, Header("路径父节点的名字(必填)")] private string pathFather;

        public LevelData CurrentData
        {
            get
            {
                levelData.PathFather = pathFather;
                return levelData;
            }
        }

        public string PathFather => pathFather;

#if UNITY_EDITOR
        [ContextMenu("Bind Data From Name")]
        private void BindDataFromName()
        {
            var str = name ?? "";
            var lines = str.Split('_');
            levelData.songID = str;
        }
#endif
    }
}
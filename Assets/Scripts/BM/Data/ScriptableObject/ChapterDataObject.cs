using UnityEngine;

namespace BM.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "BM/Chapter Data", fileName = "New Chapter Data")]
    public class ChapterDataObject : UnityEngine.ScriptableObject
    {
        [SerializeField, Header("唯一章节id不可重复 本地读取用的")] private string identifier;
        [SerializeField, Header("唯一章节id不可重复 本地读取用的")] private Sprite illustration;
        [SerializeField, Header("章节标题 如：Chapter114514")] private string chapterName;
        [SerializeField, Header("章节描述 如：治好了我的精神内耗")] private string chapterTitle;
        [SerializeField] private LevelDataObject[] levelDataObjects;
        public ChapterData CurrentData => new(identifier, illustration, chapterName, chapterTitle, levelDataObjects);
    }
}

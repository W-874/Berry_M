using BM.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BM.GameUI.ChapterSelect
{
    public class ChapterShowerControl : MonoBehaviour
    {
        [SerializeField] private Image illustration;
        [SerializeField] private Text chapterName, chapterIntroduction;
        public Image backGround;

        private Color skyBlue = new Color(0f, 1f, 1f, 0.47059f);
        private Color red = new Color(1f, 0f, 0f, 0.47059f);

        private const float Duration = 0.25f;
        //private ChapterData _chapterData;

        public void Init(ChapterData chapterData)
        {
            chapterName.text = chapterData.chapterName;
            chapterIntroduction.text = chapterData.chapterIntroduction;
            illustration.sprite = chapterData.illustrationID;
        }

        public void SetShowerAlpha(float alpha)
        {
            chapterName.color = chapterIntroduction.color = new Color(0, 0, 0, alpha);
            illustration.color = new Color(1, 1, 1, alpha);
        }
    }
}
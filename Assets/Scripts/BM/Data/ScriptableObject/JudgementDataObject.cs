using UnityEngine;

namespace BM.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "PDR/Judgement Data", fileName = "New Judgement Data")]
    public class JudgementDataObject : UnityEngine.ScriptableObject
    {
        [Header("判定相关")]
        [SerializeField] private JudgementData judgementData;

        public JudgementData CurrentData => judgementData;
    }
}

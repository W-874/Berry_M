using UnityEngine;

namespace BM.GameUI.Global
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField] private float trigger, duration;
        [SerializeField] private Vector3 from, to;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private bool isLocal, isLoop, isSmooth;

        private float time;

        private void Awake()
        {
            time = 0f;
        }

        private void Update()
        {
            time += isSmooth ? Time.smoothDeltaTime : Time.deltaTime;
            if (time < trigger || time > duration + trigger && !isLoop)
            {
                return;
            }
            var progress = curve.Evaluate(time % duration / duration);
            var trans = transform;
            if (isLocal)
            {
                trans.localEulerAngles = Vector3.Lerp(from, to, progress);
            }
            else
            {
                trans.eulerAngles = Vector3.Lerp(from, to, progress);
            }
        }
    }
}

using System.Collections;
using DG.Tweening;
using BM.Utils.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace BM.GameUI.Global
{
    public class TransitionManager : MonoSingleton<TransitionManager>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image[] transitionImage;

        public static void DoScene(string sceneName, Color transitionColor, float duration = 0f, float upTime = 0.5f, float downTime = 0.5f)
        {
            if (Instance == null)
            {
                SceneManager.LoadSceneAsync(sceneName);
                return;
            }
            Instance.StartCoroutine(Instance.CO_DoScene(sceneName, transitionColor, duration, upTime, downTime));
        }

        private IEnumerator CO_DoScene(string sceneName, Color transitionColor, float duration = 0f, float upTime = 0.5f, float downTime = 0.5f)
        {
            canvas.gameObject.SetActive(true);

            transitionImage[0].raycastTarget = true;
            transitionImage[1].raycastTarget = true;
            transitionImage[2].raycastTarget = true;

            var originColor = new Color(transitionColor.r, transitionColor.g, transitionColor.b, 0f);
            transitionImage[0].color = originColor;
            transitionImage[1].color = originColor;
            transitionImage[2].color = originColor;

            DOTween.To(() => transitionImage[0].color, x => transitionImage[0].color = x, transitionColor, upTime).SetEase(Ease.InOutSine);
            DOTween.To(() => transitionImage[1].color, x => transitionImage[1].color = x, transitionColor, upTime).SetEase(Ease.InOutSine);
            DOTween.To(() => transitionImage[2].color, x => transitionImage[2].color = x, transitionColor, upTime).SetEase(Ease.InOutSine);

            yield return new WaitForSecondsRealtime(upTime);

            var operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;

            yield return new WaitForSecondsRealtime(duration);

            operation.allowSceneActivation = true;

            yield return new WaitUntil(() => operation.isDone);

            AudioListener.pause = false;

            DOTween.To(() => transitionImage[0].color, x => transitionImage[0].color = x, originColor, downTime).SetEase(Ease.InOutSine);
            DOTween.To(() => transitionImage[1].color, x => transitionImage[1].color = x, originColor, downTime).SetEase(Ease.InOutSine);
            DOTween.To(() => transitionImage[2].color, x => transitionImage[2].color = x, originColor, downTime).SetEase(Ease.InOutSine);

            yield return new WaitForSecondsRealtime(downTime);

            transitionImage[0].raycastTarget = false;
            transitionImage[1].raycastTarget = false;
            transitionImage[2].raycastTarget = false;

            canvas.gameObject.SetActive(false);
        }
    }
}

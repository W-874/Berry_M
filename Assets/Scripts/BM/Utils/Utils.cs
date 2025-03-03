using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BM.Utils
{
    public static class Utils
    {
        public static bool TryAndCatch(Action tryToDo, Action<Exception> catchToDo = null)
        {
            try
            {
                tryToDo?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                catchToDo?.Invoke(e);
                return false;
            }
        }


        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            for (var i = 0; i < array.Length; i++)
            {
                action?.Invoke(array[i]);
            }
        }

        public static void ForEach<T>(this T[] array, Action<T, int> action)
        {
            for (var i = 0; i < array.Length; i++)
            {
                action?.Invoke(array[i], i);
            }
        }
    }

    public static class DoTweenHelper
    {
        public static Tween DoColor(this Graphic graphic, Color color, float duration)
        {
            return DOTween.To(() => graphic.color, x => graphic.color = x, color, duration);
        }

        public static Tween DoColor(this Material graphic, Color color, float duration)
        {
            return DOTween.To(() => graphic.color, x => graphic.color = x, color, duration);
        }

        public static Tween DoFade(this Graphic graphic, float alpha, float duration)
        {
            var color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            return graphic.DoColor(color, duration);
        }

        public static Tween DoFade(this CanvasGroup group, float alpha, float duration)
        {
            return DOTween.To(() => group.alpha, x => group.alpha = x, alpha, duration);
        }

        public static Tween DoVolume(this AudioSource source, float volume, float duration)
        {
            return DOTween.To(() => source.volume, x => source.volume = x, volume, duration);
        }

        public static Tween DoFill(this Image image, float fill, float duration)
        {
            return DOTween.To(() => image.fillAmount, x => image.fillAmount = x, fill, duration);
        }

        public static Tween DoSize(this Image image, Vector2 size, float duration)
        {
            return DOTween.To(() => image.rectTransform.sizeDelta, x => image.rectTransform.sizeDelta = x, size, duration);
        }
    }

    public static class UnityHelper
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}

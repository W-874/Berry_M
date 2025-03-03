using System;

namespace BM.Utils
{
    public static class EaseUtility
    {
        public enum Ease
        {
            Linear = 0,
            InSine = 1, OutSine = 2, InOutSine = 3,
            InQuad = 4, OutQuad = 5, InOutQuad = 6,
            InCubic = 7, OutCubic = 8, InOutCubic = 9,
            InQuart = 10, OutQuart = 11, InOutQuart = 12,
            InQuint = 13, OutQuint = 14, InOutQuint = 15,
            InExpo = 16, OutExpo = 17, InOutExpo = 18,
            InCirc = 19, OutCirc = 20, InOutCirc = 21,
            InElastic = 22, OutElastic = 23, InOutElastic = 24,
            InBack = 25, OutBack = 26, InOutBack = 27,
            InBounce = 28, OutBounce = 29, InOutBounce = 30,
        }

        public static float Evaluate(Ease easeType, float time, float duration, float startRange, float endRange)
        {
            var range = endRange - startRange;
            var realTime = duration * startRange + time * range;
            var startEase = Evaluate(easeType, startRange, 1f);
            var endEase = Evaluate(easeType, endRange, 1f);
            var easeRange = endEase - startEase;
            return (Evaluate(easeType, realTime, duration) - startEase) / easeRange;
        }

        public static float Evaluate(Ease easeType, float time, float duration)
        {
            float overshootOrAmplitude = 0;
            float period = 0;

            if (duration == 0) return 1f;

            float val = time / duration;
            if (val < 0f) return 0f;
            if (val > 1f) return 1f;

            switch (easeType)
            {
                case Ease.Linear:
                    return time / duration;
                case Ease.InSine:
                    return -(float)Math.Cos((double)(time / duration * 1.57079637f)) + 1f;
                case Ease.OutSine:
                    return (float)Math.Sin((double)(time / duration * 1.57079637f));
                case Ease.InOutSine:
                    return -0.5f * ((float)Math.Cos((double)(3.14159274f * time / duration)) - 1f);
                case Ease.InQuad:
                    return (time /= duration) * time;
                case Ease.OutQuad:
                    return -(time /= duration) * (time - 2f);
                case Ease.InOutQuad:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time;
                    }

                    return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
                case Ease.InCubic:
                    return (time /= duration) * time * time;
                case Ease.OutCubic:
                    return (time = time / duration - 1f) * time * time + 1f;
                case Ease.InOutCubic:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time * time;
                    }

                    return 0.5f * ((time -= 2f) * time * time + 2f);
                case Ease.InQuart:
                    return (time /= duration) * time * time * time;
                case Ease.OutQuart:
                    return -((time = time / duration - 1f) * time * time * time - 1f);
                case Ease.InOutQuart:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time * time * time;
                    }

                    return -0.5f * ((time -= 2f) * time * time * time - 2f);
                case Ease.InQuint:
                    return (time /= duration) * time * time * time * time;
                case Ease.OutQuint:
                    return (time = time / duration - 1f) * time * time * time * time + 1f;
                case Ease.InOutQuint:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time * time * time * time;
                    }

                    return 0.5f * ((time -= 2f) * time * time * time * time + 2f);
                case Ease.InExpo:
                    if (time != 0f)
                    {
                        return (float)Math.Pow(2.0, (double)(10f * (time / duration - 1f)));
                    }

                    return 0f;
                case Ease.OutExpo:
                    if (Math.Abs(time - duration) < 0.0001f)
                    {
                        return 1f;
                    }

                    return -(float)Math.Pow(2.0, (double)(-10f * time / duration)) + 1f;
                case Ease.InOutExpo:
                    if (time == 0f)
                    {
                        return 0f;
                    }

                    if (Math.Abs(time - duration) < 0.0001f)
                    {
                        return 1f;
                    }

                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * (float)Math.Pow(2.0, (double)(10f * (time - 1f)));
                    }

                    return 0.5f * (-(float)Math.Pow(2.0, (double)(-10f * (time -= 1f))) + 2f);
                case Ease.InCirc:
                    return -((float)Math.Sqrt((double)(1f - (time /= duration) * time)) - 1f);
                case Ease.OutCirc:
                    return (float)Math.Sqrt((double)(1f - (time = time / duration - 1f) * time));
                case Ease.InOutCirc:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return -0.5f * ((float)Math.Sqrt((double)(1f - time * time)) - 1f);
                    }

                    return 0.5f * ((float)Math.Sqrt((double)(1f - (time -= 2f) * time)) + 1f);
                case Ease.InElastic:
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }

                        if (Math.Abs((time /= duration) - 1f) < 0.0001f)
                        {
                            return 1f;
                        }

                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }

                        float num;
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                        {
                            num = period / 6.28318548f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
                        }

                        return -(overshootOrAmplitude * (float)Math.Pow(2.0, (double)(10f * (time -= 1f))) *
                                 (float)Math.Sin((double)((time * duration - num) * 6.28318548f / period)));
                    }
                case Ease.OutElastic:
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }

                        if (Math.Abs((time /= duration) - 1f) < 0.0001f)
                        {
                            return 1f;
                        }

                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }

                        float num2;
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num2 = period / 4f;
                        }
                        else
                        {
                            num2 = period / 6.28318548f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
                        }

                        return overshootOrAmplitude * (float)Math.Pow(2.0, (double)(-10f * time)) *
                            (float)Math.Sin((double)((time * duration - num2) * 6.28318548f / period)) + 1f;
                    }
                case Ease.InOutElastic:
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }

                        if (Math.Abs((time /= duration * 0.5f) - 2f) < 0.0001f)
                        {
                            return 1f;
                        }

                        if (period == 0f)
                        {
                            period = duration * 0.450000018f;
                        }

                        float num3;
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num3 = period / 4f;
                        }
                        else
                        {
                            num3 = period / 6.28318548f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
                        }

                        if (time < 1f)
                        {
                            return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2.0, (double)(10f * (time -= 1f))) *
                                            (float)Math.Sin((double)((time * duration - num3) * 6.28318548f / period)));
                        }

                        return overshootOrAmplitude * (float)Math.Pow(2.0, (double)(-10f * (time -= 1f))) *
                            (float)Math.Sin((double)((time * duration - num3) * 6.28318548f / period)) * 0.5f + 1f;
                    }
                case Ease.InBack:
                    return (time /= duration) * time * ((overshootOrAmplitude + 1f) * time - overshootOrAmplitude);
                case Ease.OutBack:
                    return (time = time / duration - 1f) * time *
                        ((overshootOrAmplitude + 1f) * time + overshootOrAmplitude) + 1f;
                case Ease.InOutBack:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f *
                               (time * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time - overshootOrAmplitude));
                    }

                    return 0.5f * ((time -= 2f) * time *
                        (((overshootOrAmplitude *= 1.525f) + 1f) * time + overshootOrAmplitude) + 2f);
                case Ease.InBounce:
                    return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
                case Ease.OutBounce:
                    return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
                case Ease.InOutBounce:
                    return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
                default:
                    return -(time /= duration) * (time - 2f);
            }
        }

        public static class Bounce
        {
            public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
            {
                return 1f - Bounce.EaseOut(duration - time, duration, -1f, -1f);
            }

            public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
            {
                if ((time /= duration) < 0.363636374f)
                {
                    return 7.5625f * time * time;
                }

                if (time < 0.727272749f)
                {
                    return 7.5625f * (time -= 0.545454562f) * time + 0.75f;
                }

                if (time < 0.909090936f)
                {
                    return 7.5625f * (time -= 0.8181818f) * time + 0.9375f;
                }

                return 7.5625f * (time -= 0.954545438f) * time + 0.984375f;
            }

            public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
            {
                if (time < duration * 0.5f)
                {
                    return Bounce.EaseIn(time * 2f, duration, -1f, -1f) * 0.5f;
                }

                return Bounce.EaseOut(time * 2f - duration, duration, -1f, -1f) * 0.5f + 0.5f;
            }
        }
    }
}

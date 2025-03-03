using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BM.GameUI.Settings
{
    public class HugeSlider : MonoBehaviour
    {
        [SerializeField] private Slider target;
        [SerializeField] private Text numberText;
        [SerializeField] private SliderMode sliderMode;
        [SerializeField] private Vector2 valueRange;

        public float CurrentValue
        {
            get
            {
                var value = valueRange.x + (valueRange.y - valueRange.x) * target.value;
                return sliderMode switch
                {
                    SliderMode.Percent => Mathf.Round(value * 100f) / 100f,
                    SliderMode.Int => Mathf.Round(value),
                    SliderMode.TwoPoints => Mathf.Round(value * 100f) / 100f,
                    SliderMode.Offset => value,
                    SliderMode.DSP => Mathf.Round(32f * Mathf.Pow(2f, Mathf.Round(value))),
                    _ => value
                };
            }
            set
            {
                if (sliderMode is SliderMode.DSP)
                {
                    target.value = Mathf.Clamp01((Mathf.Sqrt(value / 32f) - valueRange.x) / (valueRange.y - valueRange.x));
                    return;
                }
                target.value = Mathf.Clamp01((value - valueRange.x) / (valueRange.y - valueRange.x));
            }
        }

        private void Awake()
        {
            target.onValueChanged.AddListener(_ => ChangeText());
            ChangeText();
        }

        private void ChangeText()
        {
            var positive = CurrentValue >= 0 ? "+" : "";
            numberText.text = sliderMode switch
            {
                SliderMode.Percent => $"{Mathf.RoundToInt(CurrentValue * 100f)}%",
                SliderMode.Int => $"{Mathf.RoundToInt(CurrentValue)}",
                SliderMode.TwoPoints => $"{CurrentValue:F2}",
                SliderMode.Offset => $"{positive}{Mathf.RoundToInt(CurrentValue)}ms",
                SliderMode.DSP => $"{CurrentValue:F0}X",
                _ => $"{CurrentValue}"
            };
        }

        public void AddSlideListener(UnityAction<float> act)
        {
            target.onValueChanged.AddListener(act);
        }

        public enum SliderMode
        {
            Percent = 0, Int = 1, TwoPoints = 2, Offset = 3, DSP = 4
        }
    }
}

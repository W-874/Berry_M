using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BM.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "BM/Activity Event Data", fileName = "New Activity Data")]
    public class ActivityDataObject :  UnityEngine.ScriptableObject
    {
        [SerializeField, Header("开始时间")] private float StartTime = 0f;
        [SerializeField, Header("终止时间")] private float EndTime = 0f;
        [SerializeField, Header("展示位置")] private Vector2 Pos = Vector2.zero;
        [SerializeField, Header("大小")] private Vector3 Size = Vector2.zero;
        [SerializeField, Header("旋转")] private Vector3 Rotation = Vector2.zero;
        [SerializeField, Header("字符串展示内容")] private string text = "";
        [SerializeField, Header("图片")] private Sprite sprite;
        [SerializeField, Header("展示时颜色")] private Color color =Color.white;
        [SerializeField, Header("进出缓动曲线")] private AnimationCurve curve = new AnimationCurve();

        public int Greater(ActivityDataObject right)
        {
            return StartTime.CompareTo(right.StartTime);
        }

        public float CurrentValue(float time)
        {
            return curve.Evaluate((time-StartTime)/(EndTime-StartTime));
        }

        public bool isActivity(float time)
        {
            return time >= StartTime && time <= EndTime;
        }

        static public void UpdataObjA(GameObject from,float value)
        {
            Image fi = from.GetComponent<Image>();
            TMP_Text ft = from.transform.GetChild(0).GetComponent<TMP_Text>();
            fi.color = new Color(fi.color.r, fi.color.g, fi.color.b, value);
            ft.color = new Color(ft.color.r, ft.color.g, ft.color.b, value);
        }

        public void Init(GameObject obj)
        {
            var cat = obj.GetComponent<RectTransform>();
            cat.localPosition = Pos;
            cat.localRotation = new Quaternion() { eulerAngles = Rotation };
            cat.localScale = Size;
            var catim = cat.GetComponent<Image>();
            catim.color = color;
            if (sprite != null) catim.sprite = sprite;
            else catim.color = new Color(0, 0, 0, 0);
            SubInit(cat.GetChild(0).GetComponent<TMP_Text>());
        }

        public void SubInit(TMP_Text text)
        {
            text.color = color;
            text.text = this.text;
        }
    }
}
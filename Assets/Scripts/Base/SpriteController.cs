using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;
using UnityEngine.UI;

namespace Base
{
    public class SpriteController : UnderlyingObject
    {
        bool isEnd = false;
        bool isRandomVariations = false;
        [SerializeField]
        bool isImage = true;

        [HideInInspector]
        public Image Icat;
        [HideInInspector]
        public SpriteRenderer SRcat;
        //[SerializeField]
        public Sprite Sprite_;
        //[SerializeField]
        public Sprite LastSprite_;

        private void Awake()
        {
            if (TryGetComponent(out Image tocat))
                Icat = tocat;
            else
            {
                SRcat = GetComponent<SpriteRenderer>();
                isImage = false;
            }
            if (Icat == null && SRcat == null)
                Destroy(this);
            else
                LastSprite_ = GetNow();
        }

        public Sprite GetNow()
        {
            if (isImage)
                return Icat.sprite;
            else
                return SRcat.sprite;
        }

        public override void update()
        {
            if (isImage)
            {
                if (isRandomVariations)
                {
                    Color cat = Icat.color;
                    static float CC(float f)
                    {
                        return Mathf.Clamp(f + Random.Range(-1.0f, 1.0f) * 0.0003f, 0, 1);
                    }
                    SetColor(CC(cat.r), CC(cat.g), CC(cat.b));
                }
            }
            else
            {
                if (isRandomVariations)
                {
                    Color cat = SRcat.color;
                    static float CC(float f)
                    {
                        return Mathf.Clamp(f + Random.Range(-1.0f, 1.0f) * 0.0003f, 0, 1);
                    }
                    SetColor(CC(cat.r), CC(cat.g), CC(cat.b));
                }
            }
        }

        public void LoadFromResources(string name)
        {
            Sprite_ = Resources.Load<Sprite>(name);
            UpdateSprite();
        }

        public void SetSize(Vector3 size)
        {
            if (isImage)
                Icat.transform.localScale = size;
            else
                SRcat.transform.localScale = size;
        }

        public void SetSize(float x,float y)
        {
            SetSize(new(x, y, 1));
        }

        public void SetAColor(float a)
        {
            SetColor(1, 1, 1, a);
        }

        public void SetColor(float r = 1, float g = 1, float b = 1, float a = 1)
        {
            if (isImage)
                Icat.color = new(r, g, b, a);
            else
                SRcat.color = new(r, g, b, a);
        }

        public void UpdateSprite()
        {
            if (isImage)
            {
                LastSprite_ = Icat.sprite;
                Icat.sprite = Sprite_;
            }
            else
            {
                LastSprite_ = SRcat.sprite;
                SRcat.sprite = Sprite_;
            }
        }

        public void UpdateSprite(Sprite sprite)
        {
            if (isImage)
                Icat.sprite = sprite;
            else
                SRcat.sprite = sprite;
        }

        public void UpdateSprite2(Sprite sprite, int time = 50)
        {
            Sprite_ = sprite;
            isEnd = false;
            AddF(U, new() { Rank = time, Value = time, state = State.Active });
        }

        public void UpdateSprite2A(Sprite sprite, int time = 50)
        {
            Sprite_ = sprite;
            isEnd = false;
            AddF(UA, new() { Rank = time, Value = time, state = State.Active });
        }

        void U(Carrier carrier)
        {
            float v = carrier.Value / 2.0f;
            if (carrier.Rank-- >= v)
            {
                Vector3 cat = EasingFunction.Curve( new Vector3(0, 0, 0),new Vector3(1, 1, 1), (  carrier.Rank-v) / v);
                SetColor(cat.x, cat.y, cat.z);
            }
            else
            {
                Vector3 cat = EasingFunction.Curve(new Vector3(1, 1, 1), new Vector3(0, 0, 0), carrier.Rank / v);
                SetColor(cat.x, cat.y, cat.z);
                if (!isEnd)
                {
                    UpdateSprite();
                    isEnd = true;
                }
            }
            if (carrier.Rank <= 0)
                carrier.state = State.Destroy;
        }

        void UA(Carrier carrier)
        {
            float v = carrier.Value / 2.0f;
            if (carrier.Rank-- >= v)
            {
                float cat = EasingFunction.Curve(0, 1, (carrier.Rank - v) / v);
                SetAColor(cat);
            }
            else
            {
                float cat = EasingFunction.Curve(1, 0, carrier.Rank / v);
                SetAColor(cat);
                if (!isEnd)
                {
                    UpdateSprite();
                    isEnd = true;
                }
            }
            if (carrier.Rank <= 0)
                carrier.state = State.Destroy;
        }
    }
}

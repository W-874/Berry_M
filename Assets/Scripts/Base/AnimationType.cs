using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Base.Animation
{
    public enum State
    {
        unsafe_,
        ready,
        current,
        end
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public abstract class AnimationType<T>
    {
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AnimationTimeForm : IEquatable<AnimationTimeForm>
        {
            public float start;
            public float end;
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
            public float interval
            {
                get
                {
                    return (end - start);
                }
            }

            public AnimationTimeForm(float start, float end)
            {
                this.start = start;
                this.end = end;
            }

            public override bool Equals(object obj)
            {
                return obj is AnimationTimeForm form && Equals(form);
            }

            public bool Equals(AnimationTimeForm other)
            {
                return start == other.start &&
                       end == other.end &&
                       interval == other.interval;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(start, end, interval);
            }

            public static bool operator ==(AnimationTimeForm left, AnimationTimeForm right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(AnimationTimeForm left, AnimationTimeForm right)
            {
                return !(left == right);
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct AnimationValueForm
        {
            public T start;
            public T end;

            public AnimationValueForm(T start, T end)
            {
                this.start = start;
                this.end = end;
            }
        }


        public AnimationTimeForm time;
        public AnimationValueForm value;
        [JsonIgnore]
        [SerializeField]
        float now;
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
        public float current
        {
            get
            {
                return now;
            }
            set
            {
                now= value;
                if (value < time.start)
                    state = State.ready;
                else if(value<time.end)
                    state = State.current;
                else
                    state = State.end;
            }
        }
        [JsonIgnore] public State state = State.unsafe_;


        abstract public T GetValue();
    }

    public abstract class FlexibleType<T,R>
    { 
        public List<T> animationforms = new();

        protected FlexibleType()
        {
        }

        public void Add(T from) { animationforms.Add(from); }
        public void Add(List<T> from) { animationforms.AddRange(from); }
        public void Remove(T from) { animationforms.Remove(from); }
        public void Remove(List<T> from) { foreach (var it in from) animationforms.Remove(it); }

        abstract public R GetValue();
    
    }

    //int
    public class AnimationInt : AnimationType<int>
    {
        public EasingType curve = EasingType.Linear;

        public override int GetValue()
        {
            if (state != State.current) return 0;
            return (int)EasingFunction.Curve(value.start, value.end, (current - time.start) / (time.interval), curve);
        }
    }

    public class FlexibleInt : FlexibleType<AnimationInt,int>
    { 
        public FlexibleInt(List<AnimationInt> animationInts)
        {
            this.animationforms = animationInts;
        }

        public override int GetValue()
        {
            animationforms.Sort((S, E) => { return S.time.start.CompareTo(E.time.start); });
            foreach(var it in animationforms)
            {
                if (it.state == Base.Animation.State.current)
                    return it.GetValue();
            }
            return 0;
        }
    }

    //float
    public class AnimationFloat : AnimationType<float>
    {
        public EasingType curve = EasingType.Linear;

        public override float GetValue()
        {
            if (state != State.current) return 0;
            return EasingFunction.Curve(value.start, value.end, (current - time.start) / (time.interval), curve);
        }
    }

    public class FlexibleFloat : FlexibleType<AnimationFloat, float>
    {
        public FlexibleFloat(List<AnimationFloat> animationInts)
        {
            this.animationforms = animationInts;
        }

        public override float GetValue()
        {
            animationforms.Sort((S, E) => { return S.time.start.CompareTo(E.time.start); });
            foreach (var it in animationforms)
            {
                if (it.state == Base.Animation.State.current)
                    return it.GetValue();
            }
            return 0;
        }
    }

    //bool
    public class AnimationBool : AnimationType<bool>
    {
        public EasingType curve = EasingType.Linear;

        public override bool GetValue()
        {
            if (state != State.current) return false;
            return (EasingFunction.Curve(0, 1.0f, (current - time.start) / (time.interval), curve) >= 0.5f) ? value.start : value.end;
        }
    }

    public class FlexibleBool : FlexibleType<AnimationBool, bool>
    {
        public FlexibleBool(List<AnimationBool> animationInts)
        {
            this.animationforms = animationInts;
        }

        public override bool GetValue()
        {
            animationforms.Sort((S, E) => { return S.time.start.CompareTo(E.time.start); });
            foreach (var it in animationforms)
            {
                if (it.state == Base.Animation.State.current)
                    return it.GetValue();
            }
            return false;
        }
    }

    //vec2
    public class AnimationVec2 : AnimationType<Vector2>
    {
        public EasingType curve = EasingType.Linear;

        public override Vector2 GetValue()
        {
            if (state != State.current) return new();
            return EasingFunction.Curve(value.start, value.end, (current - time.start) / (time.interval), curve);
        }
    }

    public class FlexibleVec2 : FlexibleType<AnimationVec2, Vector2>
    {
        public FlexibleVec2(List<AnimationVec2> animationInts)
        {
            this.animationforms = animationInts;
        }

        public override Vector2 GetValue()
        {
            animationforms.Sort((S, E) => { return S.time.start.CompareTo(E.time.start); });
            foreach (var it in animationforms)
            {
                if (it.state == Base.Animation.State.current)
                    return it.GetValue();
            }
            return new();
        }
    }

    //vec3
    public class AnimationVec3 : AnimationType<Vector3>
    {
        public EasingType curve = EasingType.Linear;

        public override Vector3 GetValue()
        {
            if (state != State.current) return new();
            return EasingFunction.Curve(value.start, value.end, (current - time.start) / (time.interval), curve);
        }
    }

    public class FlexibleVec3 : FlexibleType<AnimationVec3, Vector3>
    {
        public FlexibleVec3(List<AnimationVec3> animationInts)
        {
            this.animationforms = animationInts;
        }

        public override Vector3 GetValue()
        {
            animationforms.Sort((S, E) => { return S.time.start.CompareTo(E.time.start); });
            foreach (var it in animationforms)
            {
                if (it.state == Base.Animation.State.current)
                    return it.GetValue();
            }
            return new();
        }
    }

}
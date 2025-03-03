using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class Carrier
    {
        public string BaseName = "";
        public int Rank = 0;
        public State state = State.Active;
        public float Value = 0;
    }

    public abstract class UnderlyingObject : MonoBehaviour
    {
        public float DoTimeClock;

        List<(Action<Carrier>, Carrier)> Executor = new();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:添加只读修饰符", Justification = "<挂起>")]
        List<(Action<Carrier>, Carrier)> FixedExecutor = new();

        private void Update()
        {
            update();
            for (var i = 0; i < Executor.Count;)
            {
                var it = Executor[i];
                if (it.Item2.state == State.Active)
                    it.Item1(it.Item2);
                if (it.Item2.state == State.Destroy)
                    Remove(it.Item1);
                else
                    i++;
            }
        }
        private void FixedUpdate()
        {
            fixedUpdate();
            for (int i = 0; i < FixedExecutor.Count;)
            {
                (Action<Carrier>, Carrier) it = FixedExecutor[i];
                if (it.Item2.state == State.Active)
                    it.Item1(it.Item2);
                if (it.Item2.state == State.Destroy)
                    RemoveF(it.Item1);
                else
                    i++;
            }
        }

#pragma warning disable IDE1006 // 命名样式
        public virtual void fixedUpdate() { }
        public virtual void update() { }

#pragma warning restore IDE1006 // 命名样式

        public void Add(Action<Carrier> executor, Carrier carrier)
        {
            Executor.Add((executor, carrier));
        }

        public void Add(Action<Carrier> executor)
        {
            Executor.Add((executor, new Carrier()));
        }

        public void Remove(Action<Carrier> executor, Carrier carrier)
        {
            Executor.Remove((executor, carrier));
        }

        public void Remove(int id)
        {
            Executor.RemoveAt(id);
        }

        public void Remove()
        {
            Executor.Remove(Executor[^1]);
        }

        public void Remove(Action<Carrier> executor)
        {
            foreach (var it in Executor)
                if (it.Item1 == executor)
                {
                    Executor.Remove(it);
                    break;
                }
        }
        public void AddF(Action<Carrier> executor, Carrier carrier)
        {
            Remove(executor);
            FixedExecutor.Add((executor, carrier));
        }
        public void AddF(Action<Carrier> executor)
        {
            Remove(executor);
            FixedExecutor.Add((executor, new()));
        }
        public void RemoveF(Action<Carrier> executor)
        {
            foreach (var it in FixedExecutor)
            {
                if (it.Item1 == executor)
                {
                    FixedExecutor.Remove(it);
                    break;
                }
            }
        }
        public void RemoveF(int id)
        {
            FixedExecutor.RemoveAt(id);
        }
        public bool SetState(Action<Carrier> executor, State state)
        {
            var cat = Executor.Find(T => T.Item1 == executor);
            bool t = false;
            if (cat != (null, null))
            {
                cat.Item2.state = state;
                t = true;
            }
            cat = FixedExecutor.Find(T => T.Item1 == executor);
            {
                if (cat != (null, null)) cat.Item2.state = state;
                t = true;
            }
            return t;
        }
    }


    public enum State
    {
        Preparation,
        Active,
        Ended,
        Destroy
    }
}
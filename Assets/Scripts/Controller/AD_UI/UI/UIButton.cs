using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Base;
using System;
using UnityEngine.Events;

namespace Item.UI
{
    //这里是用于Canva的Button
    [RequireComponent(typeof(SpriteController))]
    public class UIButton : MonoBehaviour, ICancelHandler, IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler
    {
        [Serializable] public class OnClickEvent : ItemEvent<int> { }//基于整数
        [Serializable] public class OnEndterEvent : ItemEvent<bool> { }
        [Serializable] public class OnExitEvent : ItemEvent<bool> { }
        public static Dictionary<string, List<UIButton>> Buttons = new();

        SpriteController sc_;
        public SpriteController SC
        {
            get
            {
                if (sc_ == null)
                    sc_ = GetComponent<SpriteController>();
                return sc_;
            }
        }
        public Animator animator;
        public string Type = "Default"; 
        public OnClickEvent OnClick = new();
        public OnEndterEvent OnEndter = new();
        public OnExitEvent OnExit = new();
        public int value = 0;
        
        private void Start()
        {
            Buttons.TryAdd(Type, new());
            Buttons[Type].Remove(this);
            Buttons[Type].Add(this);
        }

        public void OnCancel(BaseEventData eventData)
        {
            Debug.Log("OnCancel");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Type != "Default") foreach (var it in Buttons[Type]) it.Exit();
            if (Type != "Default") animator.SetBool("OnClickKeep", true);
            else StartCoroutine(OverEnter(Time.time));
            OnClick.Invoke(value);
        }

        IEnumerator OverEnter(float time)
        {
            animator.SetBool("OnClickKeep", true);
            while (Time.time < time + 1) yield return null; 
            animator.SetBool("OnClickKeep", false);
        }

        public void Exit()
        {
            animator.SetBool("OnClickKeep", false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEndter.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit.Invoke(false);
        }
    }
}
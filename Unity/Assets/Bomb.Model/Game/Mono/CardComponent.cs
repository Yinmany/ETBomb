using System;
using System.Collections.Generic;
using AkaUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bomb
{
    public class CardComponent: MonoBehaviour
    {
        public Card Card { get; set; }
        public bool IsSelect { get; set; }
        public float MoveY = 50f;

        private EventTrigger _eventTrigger;
        private static bool IsDrag { get; set; } // 是否在拖动

        public static bool Lock { get; set; }

        void Awake()
        {
            _eventTrigger = gameObject.AddComponent<EventTrigger>();
            _eventTrigger.triggers = new List<EventTrigger.Entry>();
        }

        /// <summary>
        /// 根据Card进行显示
        /// </summary>
        public void Show()
        {
            this.name = this.Card.ToString();
            Sprite sprite = this.gameObject.Get<Sprite>(this.name);
            this.GetComponent<Image>().sprite = sprite;
        }

        private void Start()
        {
            AddTrigger(EventTriggerType.PointerClick, OnPointerClick);
            AddTrigger(EventTriggerType.BeginDrag, OnBeginDrag);
            AddTrigger(EventTriggerType.Drag, OnDrag);
            AddTrigger(EventTriggerType.EndDrag, OnEndDrag);
            AddTrigger(EventTriggerType.PointerEnter, OnPointerEnter);
        }

        private void OnPointerClick()
        {
            if (!IsDrag)
            {
                ChangeSelect();
            }
        }

        private void OnBeginDrag()
        {
            IsDrag = true;
            ChangeSelect();
        }

        private void OnDrag()
        {
        }

        private void OnEndDrag()
        {
            IsDrag = false;
        }

        public void OnPointerEnter()
        {
            if (IsDrag)
            {
                ChangeSelect();
            }
        }

        /// <summary>
        /// 添加触发器
        /// </summary>
        /// <param name="eventTriggerType"></param>
        /// <param name="onTrigger"></param>
        private void AddTrigger(EventTriggerType eventTriggerType, Action onTrigger)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventTriggerType, callback = new EventTrigger.TriggerEvent() };
            entry.callback.AddListener(b => onTrigger());
            _eventTrigger.triggers.Add(entry);
        }

        /// <summary>
        /// 移除触发器
        /// </summary>
        /// <param name="eventTriggerType"></param>
        public void RemoveTrigger(EventTriggerType eventTriggerType)
        {
            for (int i = 0; i < _eventTrigger.triggers.Count; i++)
            {
                if (_eventTrigger.triggers[i].eventID == eventTriggerType)
                {
                    _eventTrigger.triggers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 使卡牌选中
        /// </summary>
        public void CardSelect()
        {
            if (!IsSelect)
            {
                ChangeSelect();
            }
        }

        /// <summary>
        /// 使卡牌不选中
        /// </summary>
        public void NotCardSelect()
        {
            if (IsSelect)
            {
                ChangeSelect();
            }
        }

        public void ChangeSelect()
        {
            if (Lock || this.enabled == false)
            {
                return;
            }

            RectTransform rect = (RectTransform) transform;
            if (IsSelect)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0);
                IsSelect = false;
            }
            else
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, MoveY);
                IsSelect = true;
            }
        }
    }
}
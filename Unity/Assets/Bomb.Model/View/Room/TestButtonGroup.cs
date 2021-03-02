using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AkaUI;
using UnityEngine;

namespace Bomb.View
{
    public partial class TestButtonGroup: UIPanel
    {
        private bool isCards = false;

        protected override void OnCreate()
        {
            this._addPlayer.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Add).Coroutine(); });

            this._removePlayer.onClick.AddListener(() => { LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Remove).Coroutine(); });
            this._readyPlayer.onClick.AddListener(() => LobbyPlayer.Ins.RoomOp(RoomOpType.MockPlayer_Ready).Coroutine());
            this._postPlayer.onClick.AddListener(() =>
            {
                string cardText = this._inputField.text;
                Dictionary<string, CardWeight> map = new Dictionary<string, CardWeight>();
                Type type = typeof (CardWeight);
                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    var attr = fieldInfo.GetCustomAttribute<DescriptionAttribute>(false);
                    if (attr == null)
                    {
                        continue;
                    }

                    map.Add(attr.Description, (CardWeight) Enum.ToObject(type, fieldInfo.GetValue(type)));
                }

                cardText = cardText.Replace("大", "大王");
                cardText = cardText.Replace("小", "小王");

                List<Card> cards = new List<Card>();
                foreach (char c in cardText)
                {
                    if (map.TryGetValue(c.ToString(), out var weight))
                    {
                        cards.Add(new Card { Weight = weight });
                    }
                }

                LobbyPlayer.Ins.RoomOp(RoomOpType.MockCards, cards).Coroutine();
            });
        }
    }
}
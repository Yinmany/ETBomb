using System;
using System.Collections.Generic;
using System.Linq;
using AkaUI;
using Bomb.View;
using ET;
using GameEventType;

namespace Bomb
{
    public class LobbyPlayerAwakeSystem: AwakeSystem<LobbyPlayer>
    {
        public override void Awake(LobbyPlayer self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 大厅玩家
    /// </summary>
    public class LobbyPlayer: Entity
    {
        public static LobbyPlayer Ins { get; private set; }

        public void Awake()
        {
            Ins = this;
        }

        public void OnLobbyPlayerInfoChanged()
        {
            EventBus.Publish(new LobbyPlayerInfoChanged());
        }

        public async ETVoid CreateRoom()
        {
            try
            {
                NetworkLoading.Open();
                var resp = (G2C_CreateRoomResponse) await SessionComponent.Instance.Session.Call(new C2G_CreateRoomRequest());
                if (resp.RoomNum > 0)
                {
                    await this.EnterRoom(resp.RoomNum);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                NetworkLoading.Close();
            }
        }

        public async ETTask EnterRoom(long roomNum)
        {
            G2C_EnterRoomResponse response =
                    (G2C_EnterRoomResponse) await SessionComponent.Instance.Session.Call(new C2G_EnterRoomRequest { RoomNum = roomNum });

            switch (response.Error)
            {
                case ErrorCode.ERR_Success:
                {
                    EventSystem.Instance.Publish(new EnterRoomSuccess
                    {
                        RoomNum = roomNum, RoomMaster = response.RoomMaster, CurrentPlayerSeatIndex = response.SeatIndex
                    });

                    break;
                }
                case GameErrorCode.ERR_NotFoundRoom:
                {
                    Dialog.Open(new Dialog.Args("加入房间", $"无法找到房间:{roomNum}!"));
                    break;
                }
                case GameErrorCode.ERR_RoomNotSeat:
                {
                    Dialog.Open(new Dialog.Args("加入房间", $"房间已满!"));
                    break;
                }
            }
        }

        public async ETTask ExitRoom(bool isDestroy = false)
        {
            var resp = (G2C_ExitRoomResponse) await SessionComponent.Instance.Session.Call(new C2G_ExitRoomRequest() { IsDestroyRoom = isDestroy });

            // 只再退出房间是，切换UI。销毁的话就走RoomOpMessage
            if (resp.Error == ErrorCode.ERR_Success && !isDestroy)
            {
                Game.EventSystem.Publish(new ExitRoom());
            }
        }

        public async ETTask RoomOp(int op, List<Card> cards = null)
        {
            var msg = new C2M_RoomOpRequest() { Op = op };
            if (cards != null)
            {
                msg.Cards.AddRange(cards.Select(f => new CardProto { Color = (int) f.Color, Weight = (int) f.Weight }));
            }

            var resp = (M2C_RoomOpResponse) await SessionComponent.Instance.Session.Call(msg);
        }
    }
}
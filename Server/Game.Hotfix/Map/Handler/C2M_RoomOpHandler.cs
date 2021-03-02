using System;
using System.Collections.Generic;
using System.Linq;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class C2M_RoomOpHandler: AMActorLocationRpcHandler<Player, C2M_RoomOpRequest, M2C_RoomOpResponse>
    {
        protected override async ETTask Run(Player player, C2M_RoomOpRequest message, M2C_RoomOpResponse response, Action reply)
        {
            Room room = player.Room;

            switch (message.Op)
            {
                case RoomOpType.Ready:
                    room.Ready(player);
                    break;

                case RoomOpType.CancelReady:
                    room.Ready(player, false);
                    break;
                case RoomOpType.NotPlay:
                    player.NotPop();
                    break;
                case RoomOpType.Destroy:

                    break;
                case RoomOpType.MockCards:

                    var test = player.GetComponent<TestCardComponent>();
                    if (test == null)
                    {
                        test = player.AddComponent<TestCardComponent>();
                    }

                    test.Cards = message.Cards;
                    test.Seat = player.SeatIndex;
                    break;

                case RoomOpType.MockPlayer_Add:

                    if (room.TryGetSeatIndex(out int seat))
                    {
                        Player mockPlayer = EntityFactory.Create<Player, long, Room>(room.Domain, 0, room);
                        mockPlayer.AddComponent<MailBoxComponent>().IgnoreNotFondHandlerException = true;
                        mockPlayer.SeatIndex = seat;
                        var playerServer = mockPlayer.AddComponent<PlayerServer, long>(mockPlayer.InstanceId);
                        playerServer.Flags = PlayerFlags.Robot;
                        mockPlayer.AddComponent<RobotProxy>();
                        var mockModel = mockPlayer.AddComponent<PlayerModel>();
                        mockModel.NickName = "T" + RandomHelper.RandomNumber(1, 1000);
                        room.Enter(mockPlayer);
                    }

                    break;
                case RoomOpType.MockPlayer_Remove:
                    for (int i = 0; i < room.Players.Length; i++)
                    {
                        var p = room.Players[i];

                        if (p == null || p.GetComponent<PlayerServer>().Flags == PlayerFlags.Player)
                        {
                            continue;
                        }

                        room.Exit(p);
                        break;
                    }

                    break;
                case RoomOpType.MockPlayer_Ready:
                    for (int i = 0; i < room.Players.Length; i++)
                    {
                        var p = room.Players[i];
                        if (p == null || p.GetComponent<PlayerServer>().Flags == PlayerFlags.Player)
                        {
                            continue;
                        }

                        room.Ready(p);
                    }

                    break;
                case RoomOpType.MockPlayer_CancelReady:

                    break;
                case RoomOpType.MockPlayer_SwitchSeat:
                    break;
            }

            reply();

            await ETTask.CompletedTask;
        }
    }
}
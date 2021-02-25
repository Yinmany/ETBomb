using System;
using System.Linq;
using ET;

namespace Bomb
{
    [ActorMessageHandler]
    public class C2M_PlayerPlayCardHandler: AMActorLocationRpcHandler<Player, C2M_PlayCardRequest, M2C_PlayCardResponse>
    {
        protected override async ETTask Run(Player player, C2M_PlayCardRequest message, M2C_PlayCardResponse response, Action reply)
        {
            bool isTrue = player.Pop(message.Cards.Select(f => new Card { Color = (CardColor) f.Color, Weight = (CardWeight) f.Weight }).ToList());
            response.Error = isTrue? ErrorCode.ERR_Success : GameErrorCode.ERR_PlayCard;
            reply();
            await ETTask.CompletedTask;
        }
    }
}
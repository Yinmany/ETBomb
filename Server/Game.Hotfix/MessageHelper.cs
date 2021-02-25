namespace Bomb
{
    public class MessageHelper
    {
        // public static void Broadcast(Unit unit, IActorMessage message)
        // {
        //     var units = unit.Domain.GetComponent<UnitComponent>().GetAll();
        //
        //     if (units == null) return;
        //
        //     foreach (Unit u in units)
        //     {
        //         UnitGateComponent unitGateComponent = u.GetComponent<UnitGateComponent>();
        //         if (unitGateComponent.IsDisconnect)
        //         {
        //             continue;
        //         }
        //
        //         ActorMessageHelper.SendActor(unitGateComponent.GateSessionActorId, message);
        //     }
        // }
    }
}
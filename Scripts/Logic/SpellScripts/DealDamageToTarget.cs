using UnityEngine;
using System.Collections;

public class DealDamageToTarget : SpellEffect 
{
    

    public override void ActivateEffect(int specialAmount = 0, IUnit target = null, int numberOfTurns = 0)
    {
        new DealDamageCommand(target.ID, specialAmount, healthAfter: target.Health - specialAmount).AddToQueue();
        target.Health -= specialAmount;
    }
}

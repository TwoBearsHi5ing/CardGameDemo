using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, IUnit target = null, int numberOfTurns = 0)
    {
        new HealCommand(target.ID, specialAmount, healthAfter: target.Health + specialAmount).AddToQueue();
        target.Health += specialAmount;
    }



}

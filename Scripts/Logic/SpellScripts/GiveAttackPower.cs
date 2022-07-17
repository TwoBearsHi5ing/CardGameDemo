using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAttackPower : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, IUnit target = null, int numberOfTurns = 0)
    {
        new AddAttackCommand(target.ID, specialAmount, _attack: target.Attack + specialAmount).AddToQueue();
        target.Attack += specialAmount;
    }
}

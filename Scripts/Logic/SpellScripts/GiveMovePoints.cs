using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMovePoints : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, IUnit target = null, int numberOfTurns = 0)
    {
        new AddMovePointsCommand(target.ID, specialAmount, movesAfter: target.MovePoints + specialAmount).AddToQueue();


        target.MovePoints += specialAmount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCard : UnitEffect
{
    public BonusCard(Player owner, UnitInLogic unit, int specialAmount) : base(owner, unit, specialAmount)
    { }

    public override void WhenUnitIsPlayed()
    {
        TurnManager.Instance.whoseTurn.DrawACard(true);
    }

}

using UnityEngine;
using System.Collections;

public class DamageEnemyHeroOnPlay : UnitEffect
{
    public DamageEnemyHeroOnPlay(Player owner, UnitInLogic unit, int specialAmount): base(owner, unit, specialAmount)
    {}

    // BATTLECRY
    public override void WhenUnitIsPlayed()
    {
        int id = 0;

        // Debug.Log("InCauseEffect: owner: "+ owner + " specialAmount: "+ specialAmount);
        if (unit.owner.tag == "LowPlayer")
        {
            id = 4;
        }
        else if (unit.owner.tag == "TopPlayer")
        {
            id = 6;
        }
        else
        {
            //Debug.LogError("Blad! : zle id");
        }
        if (id > 0 && id == 4 || id == 6)
        {
            UnitInLogic hero = UnitInLogic.FindUnitLogicByID(id);

            new DealDamageCommand(id, specialAmount, healthAfter: hero.Health - specialAmount).AddToQueue();
            hero.Health -= specialAmount;
        }


       
    }
}

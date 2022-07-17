using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDeathDamage : UnitEffect
{
    public AfterDeathDamage(Player owner, UnitInLogic unit, int specialAmount) : base(owner, unit, specialAmount)
    { }



    public override void WhenUnitDies()
    {
        List<UnitInLogic> creaturesToDamage = new List<UnitInLogic>();

        for (int i = 0; i < Table.instance.UnitsOnTable.Count; i++)
        {
            if (Table.instance.UnitsOnTable[i].owner == owner.otherPlayer)
            {
                creaturesToDamage.Add(Table.instance.UnitsOnTable[i]);
            }
        }

        System.Random random = new System.Random();

        int cre = random.Next(0, creaturesToDamage.Count);  

        try
        {
            int id = creaturesToDamage[cre].UniqueUnitID;
            if (id > 0)
            {
                UnitInLogic toDamage = UnitInLogic.FindUnitLogicByID(id);

                new DealDamageCommand(id, specialAmount, healthAfter: toDamage.Health - specialAmount).AddToQueue();
                toDamage.Health -= specialAmount;
            }
        }
        catch
        {
        }
    }
}

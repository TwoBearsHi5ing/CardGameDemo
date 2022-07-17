using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFriendlyUnit : UnitEffect
{
    public HealFriendlyUnit(Player owner, UnitInLogic unit, int specialAmount) : base(owner, unit, specialAmount)
    { }


    public override void WhenUnitIsPlayed()
    {
        List<UnitInLogic> creatureToHeal = new List<UnitInLogic>();

        for (int i = 0; i < Table.instance.UnitsOnTable.Count; i++)
        {
            if (Table.instance.UnitsOnTable[i].owner == owner)
            {
                if (Table.instance.UnitsOnTable[i].ID != unit.ID)
                {
                    creatureToHeal.Add(Table.instance.UnitsOnTable[i]);
                }
              
            }
        }

        System.Random random = new System.Random();
        int cre = random.Next(0, creatureToHeal.Count);  

        try
        {
            int id = creatureToHeal[cre].UniqueUnitID;
            if (id > 0)
            {
                UnitInLogic toHeal = UnitInLogic.FindUnitLogicByID(id);

                new HealCommand(id, specialAmount, healthAfter: toHeal.Health + specialAmount).AddToQueue();
                toHeal.Health += specialAmount;
            }
        }
        catch
        {
        }


    }
}

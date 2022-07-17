using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAllUnits : TableSpellEffect
{
    public HealAllUnits(Player owner, SpellInLogic spell, int specialAmount, int numberOfTurns) : base(owner, spell, specialAmount, numberOfTurns)
    { }

    public override void RegisterEventEffect()
    {
        owner.StartTurnEvent += CauseEventEffect;
    }

    public override void UnRegisterEventEffect()
    {
        owner.StartTurnEvent -= CauseEventEffect;
    }

    public override void CauseEventEffect()
    {
        List<UnitInLogic> creaturesToDamage = new List<UnitInLogic>();

        for (int i = 0; i < Table.instance.UnitsOnTable.Count; i++)
        {
            if (Table.instance.UnitsOnTable[i].owner == owner)
            {
                creaturesToDamage.Add(Table.instance.UnitsOnTable[i]);
            }
        }


        foreach (UnitInLogic cl in creaturesToDamage)
        {
            new HealCommand(cl.ID, specialAmount, healthAfter: cl.Health + specialAmount).AddToQueue();
            cl.Health += specialAmount;
        }

    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageAllOpponentUnits : TableSpellEffect
{
    public DamageAllOpponentUnits(Player owner, SpellInLogic spell, int specialAmount, int numberOfTurns) : base(owner, spell, specialAmount, numberOfTurns)
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
            if (Table.instance.UnitsOnTable[i].owner == owner.otherPlayer)
            {
                creaturesToDamage.Add(Table.instance.UnitsOnTable[i]);
            }
        }


        foreach (UnitInLogic cl in creaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }

    }

}

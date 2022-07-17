using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRandomUnitSpell : TableSpellEffect
{
    public DamageRandomUnitSpell(Player owner, SpellInLogic spell, int specialAmount, int numberOfTurns) : base(owner, spell, specialAmount, numberOfTurns)
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
            if (Table.instance.UnitsOnTable[i].owner != null)
            {
                creaturesToDamage.Add(Table.instance.UnitsOnTable[i]);
            }
        }

        System.Random random = new System.Random();

        int cre = random.Next(0, creaturesToDamage.Count);
        //Debug.LogWarning("wylosowana jednostka: " + cre);

        try
        {
            int id = creaturesToDamage[cre].UniqueUnitID;
          //  Debug.LogWarning("wylosowana jednostka: id  " + creaturesToDamage[cre].UniqueCreatureID);

            if (id != 0)
            {
                UnitInLogic toDamage = UnitInLogic.FindUnitLogicByID(id);

                new DealDamageCommand(id, specialAmount, healthAfter: toDamage.Health - specialAmount).AddToQueue();
                toDamage.Health -= specialAmount;
            }
        }
        catch
        {
            //Debug.LogWarning("Brak jednostek na planszy " );
        }
        
        
       
    }
}
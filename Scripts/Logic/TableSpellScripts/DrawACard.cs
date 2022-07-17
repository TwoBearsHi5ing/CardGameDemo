using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawACard : TableSpellEffect
{

    public DrawACard(Player owner, SpellInLogic spell, int specialAmount, int numberOfTurns) : base(owner, spell, specialAmount, numberOfTurns)
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
        TurnManager.Instance.whoseTurn.DrawACard(true);

    }

}

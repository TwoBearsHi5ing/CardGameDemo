using UnityEngine;
using System.Collections;

public class GiveApBonus: TableSpellEffect 
{
    public GiveApBonus(Player owner, SpellInLogic spell, int specialAmount, int numberOfTurns) : base(owner, spell, specialAmount, numberOfTurns)
    { }

    public override void WhenASpellIsPlayed()
    {
        TurnManager.Instance.whoseTurn.GetApBonus(specialAmount);
    }
}

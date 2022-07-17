using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSpellEffect
{
    protected Player owner;
    protected SpellInLogic spell;
    protected int specialAmount;
    protected int NumberOfTurns;

    public TableSpellEffect(Player owner, SpellInLogic spell, int specialAmount, int numberOfTurns)
    {
        this.spell = spell;
        this.owner = owner;
        this.specialAmount = specialAmount;
        NumberOfTurns = numberOfTurns;
    }

    public virtual void ActivateEffect(int specialAmount = 0, IUnit target = null, int numberOfTurns = 1)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }

    public virtual void RegisterEventEffect() { }

    public virtual void UnRegisterEventEffect() { }

    public virtual void CauseEventEffect() { }

    public virtual void WhenASpellIsPlayed() { }

    public virtual void WhenSpellExpires() { }
}

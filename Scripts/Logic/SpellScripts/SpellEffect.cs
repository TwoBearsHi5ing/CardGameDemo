using UnityEngine;
using System.Collections;

public abstract class SpellEffect
{
    public virtual void ActivateEffect(int specialAmount = 0, IUnit target = null, int numberOfTurns = 50)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }
    public virtual void RegisterEventEffect() { }

    public virtual void UnRegisterEventEffect() { }

    public virtual void CauseEventEffect() { }

    public virtual void WhenASpellIsPlayed() { }

    public virtual void WhenSpellExpires() { }
}

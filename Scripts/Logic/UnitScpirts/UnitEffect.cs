using UnityEngine;
using System.Collections;

public abstract class UnitEffect 
{
    protected Player owner;
    protected UnitInLogic unit;
    protected int specialAmount;

    public UnitEffect(Player owner, UnitInLogic unit, int specialAmount)
    {
        this.unit = unit;
        this.owner = owner;
        this.specialAmount = specialAmount;
    }

    public virtual void RegisterEventEffect(){}

    public virtual void UnRegisterEventEffect(){}

    public virtual void CauseEventEffect(){}

    public virtual void WhenUnitIsPlayed(){}

    public virtual void WhenUnitDies(){}


}

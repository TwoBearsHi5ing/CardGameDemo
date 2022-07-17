using UnityEngine;
using System.Collections;

public class BiteOwner : UnitEffect
{  
    public BiteOwner(Player owner, UnitInLogic unit, int specialAmount): base(owner, unit, specialAmount)
    {}

    public override void RegisterEventEffect()
    {
        owner.EndTurnEvent += CauseEventEffect;
    }

    public override void UnRegisterEventEffect()
    {
        owner.EndTurnEvent -= CauseEventEffect;
    }

    public override void CauseEventEffect()
    {
        int id = 0;
      
        if (owner.tag == "LowPlayer")
        {
            id = 6;
        }
        else if (owner.tag == "TopPlayer")
        {
            id = 4;
        }
        else
        {
            Debug.LogError("Blad! : zle id");
        }
        if (id > 0 && id == 4 || id == 6)
        {
            UnitInLogic hero = UnitInLogic.FindUnitLogicByID(id);
            new DealDamageCommand(id, specialAmount, healthAfter: hero.Health - specialAmount).AddToQueue();
            hero.Health -= specialAmount;
        }
      
    }


}

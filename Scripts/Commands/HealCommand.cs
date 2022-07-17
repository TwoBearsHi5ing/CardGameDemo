using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCommand : Command
{
    private int targetID;
    private int amount;
    private int healthAfter;

    public HealCommand(int targetID, int amount, int healthAfter)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (targetID == 4 || targetID == 6)
        {
            target.GetComponent<OneHeroManager>().Heal(amount, healthAfter);
        }
        else
        {
            target.GetComponent<OneUnitManager>().Heal(amount, healthAfter);
        }
        CommandExecutionComplete();
    }




}

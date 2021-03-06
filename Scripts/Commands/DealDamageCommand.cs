using UnityEngine;
using System.Collections;

public class DealDamageCommand : Command {

    private int targetID;
    private int amount;
    private int healthAfter;

    public DealDamageCommand( int targetID, int amount, int healthAfter)
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
            target.GetComponent<OneHeroManager>().TakeDamage(amount,healthAfter);
        }
        else
        {
            target.GetComponent<OneUnitManager>().TakeDamage(amount, healthAfter);
        }
        CommandExecutionComplete();
    }
}

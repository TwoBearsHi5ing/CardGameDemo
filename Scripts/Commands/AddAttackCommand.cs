using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAttackCommand : Command
{

    private int targetID;
    private int amount;
    private int Attack;

    public AddAttackCommand(int targetID, int amount, int _attack)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.Attack = _attack;
    }

    public override void StartCommandExecution()
    {
        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (targetID == 4 || targetID == 6)
        {
            target.GetComponent<OneHeroManager>().AddAttack(amount, Attack);
        }
        else
        {
            target.GetComponent<OneUnitManager>().AddAttack(amount, Attack);
        }
        CommandExecutionComplete();
    }
}

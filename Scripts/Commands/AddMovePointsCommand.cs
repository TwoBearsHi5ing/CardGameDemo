using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMovePointsCommand : Command
{

    private int targetID;
    private int amount;
    private int Movepoints;
    private int TurnsNumber;

    public AddMovePointsCommand(int targetID, int amount, int movesAfter)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.Movepoints = movesAfter;

    }

    public override void StartCommandExecution()
    {

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (targetID == 4 || targetID == 6)
        {
            target.GetComponent<OneHeroManager>().AddMovePoints(amount, Movepoints);
        }
        else
        {
            target.GetComponent<OneUnitManager>().AddMovePoints(amount, Movepoints);
        }
        CommandExecutionComplete();
    }
}

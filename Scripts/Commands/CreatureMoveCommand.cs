using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMoveCommand : Command
{
    private int TargetUniqueID;
    private int MovePointsLeft;
    private Vector3 newPos;

    public CreatureMoveCommand(int targetID, Vector3 NewPosition, int MovePoints )
    {
        TargetUniqueID = targetID;
        newPos = NewPosition;
        MovePointsLeft = MovePoints;
        
    }

    public override void StartCommandExecution()
    {
        GameObject CreatureToMove = IDHolder.GetGameObjectWithID(TargetUniqueID);

        CreatureToMove.GetComponent<CreatureMoveVisual>().MoveTarget(TargetUniqueID, newPos, MovePointsLeft);
      
    }
}

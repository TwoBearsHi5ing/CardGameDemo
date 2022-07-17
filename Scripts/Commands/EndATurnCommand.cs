using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndATurnCommand : Command
{
    public override void StartCommandExecution()
    {
        TurnManager.Instance.EndTurn();
        CommandExecutionComplete();
    }
}

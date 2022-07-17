using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Command 
{
    private Player p;
    private int DeadCreatureID;
    private int DeadCreatureIndex;

    public CreatureDieCommand(int CreatureID, Player p, int index)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
        DeadCreatureIndex = index;
    }

    public override void StartCommandExecution()
    {
        p.PArea.DualTableVisual.RemoveCreatureWithID(DeadCreatureID, DeadCreatureIndex);
    }
}

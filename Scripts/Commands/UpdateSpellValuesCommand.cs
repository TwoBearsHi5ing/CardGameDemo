using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSpellValuesCommand : Command
{
    private int _turnsLeft;
   
    private int _id;

    public UpdateSpellValuesCommand(int turnsLeft, int id)
    {
        _turnsLeft = turnsLeft;
        _id = id;
    }
    public override void StartCommandExecution()
    {
       

        GameObject target = IDHolder.GetGameObjectWithID(_id);
      
        target.GetComponent<OneSpellManager>().updateStats(_turnsLeft);
      

        CommandExecutionComplete();
    }
}

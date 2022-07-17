using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCreatureValuesCommand : Command
{
    private int _attack;
    private int _movePoints;
    private int _health;
    private int _id;

    public UpdateCreatureValuesCommand(int atk, int mp, int hp, int id)
    {
        _attack = atk;
        _movePoints = mp;
        _health = hp;
        _id = id;
    }
    public override void StartCommandExecution()
    {
       

        GameObject target = IDHolder.GetGameObjectWithID(_id);
        if (_id == 4 || _id == 6)
        {
            target.GetComponent<OneHeroManager>().updateStats(_attack,_movePoints,_health);
        }
        else
        {
            target.GetComponent<OneUnitManager>().updateStats(_attack, _movePoints, _health);
        }

        CommandExecutionComplete();
    }
}

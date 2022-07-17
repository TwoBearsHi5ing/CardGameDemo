using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Command 
{
  
    private int TargetUniqueID;
    private int AttackerUniqueID;
    private int AttackerHealthAfter;
    private int TargetHealthAfter;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;
    private int MovePointsAfterAction;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter, int movePointsAfter)
    {
        TargetUniqueID = targetID;
        AttackerUniqueID = attackerID;
        AttackerHealthAfter = attackerHealthAfter;
        TargetHealthAfter = targetHealthAfter;
        DamageTakenByTarget = damageTakenByTarget;
        DamageTakenByAttacker = damageTakenByAttacker;
        MovePointsAfterAction = movePointsAfter;
    }

    public override void StartCommandExecution()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);
        Attacker.GetComponent<CreatureAttackVisual>().AttackTarget(TargetUniqueID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter , MovePointsAfterAction);
    }
}

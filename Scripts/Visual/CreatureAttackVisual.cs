using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CreatureAttackVisual : MonoBehaviour 
{
    private OneUnitManager manager;
    private OneHeroManager h_manager;
    private WhereIsTheCardOrCreature w;

    void Awake()
    {
        manager = GetComponent<OneUnitManager>();
        h_manager = GetComponent<OneHeroManager>();
        w = GetComponent<WhereIsTheCardOrCreature>();
    }

    public void AttackTarget(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackerHealthAfter, int targetHealthAfter, int MovePointsAfterAttack)
    {
        Debug.Log(targetUniqueID);
        if (manager != null)
        {
            manager.CanAttackNow = false;
        }
        else if (h_manager != null)
        {
            h_manager.CanAttackNow = false;
        }

        GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

       
        w.BringToFront();
        w.VisualState = VisualStates.Transition;

        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if(damageTakenByTarget>0)
                    DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget, Visual_Effect.Damage);
                if(damageTakenByAttacker>0)
                    DamageEffect.CreateDamageEffect(transform.position, damageTakenByAttacker, Visual_Effect.Damage);

               

                if (targetUniqueID == 4 || targetUniqueID == 6)
                {
                    // target is a player
                    target.GetComponent<OneHeroManager>().HealthText.text = targetHealthAfter.ToString();
                }
                else
                {
                    target.GetComponent<OneUnitManager>().HealthText.text = targetHealthAfter.ToString();
                }

                w.SetTableSortingOrder();
                w.VisualState = VisualStates.Table;

                if (manager != null)
                {
                    manager.HealthText.text = attackerHealthAfter.ToString();
                    manager.MovePoints.text = MovePointsAfterAttack.ToString();
                }

                if (h_manager != null)
                {
                    h_manager.HealthText.text = attackerHealthAfter.ToString();
                    h_manager.MovePoints.text = MovePointsAfterAttack.ToString();
                }

                Sequence s = DOTween.Sequence();
                s.AppendInterval(1f);
                s.OnComplete(Command.CommandExecutionComplete);
                //Command.CommandExecutionComplete();
            });
    }
        
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMoveVisual : MonoBehaviour
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

    public void MoveTarget(int UniqueID, Vector3 pos, int MovePointsLeftAfterMoving)
    {
       

        GameObject target = IDHolder.GetGameObjectWithID(UniqueID);

        // bring this creature to front sorting-wise.
        w.BringToFront();
        w.VisualState = VisualStates.Transition;

       

        transform.DOMove(pos, 0.5f).OnComplete(() =>
        {
            if (manager != null)
            {
                manager.MovePoints.text = MovePointsLeftAfterMoving.ToString();
            }

            if (h_manager != null)
            {
                h_manager.MovePoints.text = MovePointsLeftAfterMoving.ToString();
            }



            w.SetTableSortingOrder();
            w.VisualState = VisualStates.Table;           

            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.0f);
            s.OnComplete(Command.CommandExecutionComplete);
            //Command.CommandExecutionComplete();
        });
    }
}

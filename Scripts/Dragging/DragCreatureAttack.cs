using System.Collections.Generic;
using UnityEngine;

public class DragCreatureAttack : DraggingActions
{

    // reference to the sprite with a round "Target" graphic
    private SpriteRenderer sr;
    // LineRenderer that is attached to a child game object to draw the arrow
    private LineRenderer lr;
    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrCreature whereIsThisCreature;
    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    private Transform triangle;
    // SpriteRenderer of triangle. We need this to disable the pointy end if the target is too close.
    private SpriteRenderer triangleSR;
    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;
    // Reference to creature manager, attached to the parent game object
    private OneUnitManager manager;
    private OneHeroManager Heromanager;

    private UnitInLogic you;
    private List<int> AllovedSlotsToMove = new List<int>();
    private List<int> AllovedSlotsToAttack = new List<int>();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Arrow");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneUnitManager>();
        Heromanager = GetComponentInParent<OneHeroManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
           

            if (manager == null)
            {
                return base.CanDrag && Heromanager.CanAttackNow;
            }
            else
            {
                return base.CanDrag && manager.CanAttackNow;
            }


        }
    }
    


    public override void OnStartDrag()
    {
      

        int Id = GetComponentInParent<IDHolder>().UniqueID;
        you = UnitInLogic.FindUnitLogicByID(Id);

        int Position = you.Position;
        int AttackRange;
        int MoveRange;
        MovementOptions MoveOption;
        MovementOptions AttackOption;

        if (manager != null)
        {
            MoveRange = manager.cardAsset.MoveRange;
            AttackRange = manager.cardAsset.AttackRange;
            MoveOption = manager.cardAsset.moveOption;
            AttackOption = manager.cardAsset.attackOption;
        }
        else
        {
            MoveRange = Heromanager.heroAsset.MoveRange;
            AttackRange = Heromanager.heroAsset.AttackRange;
            MoveOption = Heromanager.heroAsset.moveOption;
            AttackOption = Heromanager.heroAsset.attackOption;
        }

        whereIsThisCreature.VisualState = VisualStates.Dragging;
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;


        AllovedSlotsToMove = GetAllowedMoves.instance.GetAllowedSlots(Position, MoveRange, MoveOption);
        AllovedSlotsToAttack = GetAllowedMoves.instance.GetAllowedSlots(Position, AttackRange, AttackOption);

        if (AllovedSlotsToMove != null)
        {
            TabeSlots.Instance.ChangeSlotColor(true, AllovedSlotsToMove);
        }

        if (AllovedSlotsToAttack != null)
        {
            TabeSlots.Instance.ChangeFrameColor(true, AllovedSlotsToAttack);
        }


    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.parent.position = transform.position -  direction;

       
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }

    }

    public override void OnEndDrag()
    {
        if (AllovedSlotsToMove != null)
        {
            TabeSlots.Instance.ReturnDefaultSlotColor(AllovedSlotsToMove);
        }
        if (AllovedSlotsToAttack != null)
        {
            TabeSlots.Instance.ReturnDefaulFrameColor(AllovedSlotsToAttack);
        }

        bool allowToAttack = false;
        bool allowToMove = false;
        bool found_a_target = false;
        Vector3 tablePos;
        int tableindex;
      

        tableindex = playerOwner.PArea.DualTableVisual.TablePositionForNewUnit(Camera.main.ScreenToWorldPoint(
              new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x, (Camera.main.ScreenToWorldPoint(
              new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).y));

        Target = null;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        foreach (RaycastHit h in hits)
        {

            if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreature") ||
                (h.transform.tag == "LowPlayer" && this.tag == "TopCreature"))
            {
                found_a_target = true;
                Target = h.transform.gameObject;
                Debug.Log("Attacking " + Target);
            }
            else if ((h.transform.tag == "TopCreature" && this.tag == "LowCreature") ||
                    (h.transform.tag == "LowCreature" && this.tag == "TopCreature"))
            {
                found_a_target = true;
                Target = h.transform.parent.gameObject;
                Debug.Log("Attacking 2" + Target);
            }


        }

        bool targetValid = false;

        if (AllovedSlotsToAttack != null)
        {
            for (int i = 0; i < AllovedSlotsToAttack.Count; i++)
            {
                if (AllovedSlotsToAttack[i] == tableindex)
                {
                    allowToAttack = true;
                }
            }
        }

        if (Target != null && found_a_target && allowToAttack)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
               
                UnitInLogic.UnitsCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackUnitWithID(targetID);
                targetValid = true;
            }
            else if (UnitInLogic.UnitsCreatedThisGame[targetID] != null)
            {
                targetValid = true;
                UnitInLogic.UnitsCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackUnitWithID(targetID);
             
            }

        }

        else if (Target == null)
        {
            if (AllovedSlotsToMove != null)
            {
                for (int i = 0; i < AllovedSlotsToMove.Count; i++)
                {
                    if (AllovedSlotsToMove[i] == tableindex)
                    {
                        allowToMove = true;
                    }
                }
            }



            tablePos = playerOwner.PArea.DualTableVisual.TablePosForCreature(Camera.main.ScreenToWorldPoint(
             new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x, (Camera.main.ScreenToWorldPoint(
             new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).y));

           
          

            if (tableindex >= 0 && tableindex < 24 && allowToMove)
            {
                if (Table.instance.ChechkIfUnitSlotIsFree(tableindex))
                {
                    int id = GetComponentInParent<IDHolder>().UniqueID;
                    UnitInLogic.UnitsCreatedThisGame[id].MoveUnit(id, tableindex, tablePos);
                    targetValid = true;
                }
            }
        }

        if (!targetValid)
        {
            //  Debug.Log("Nie dziala " + Target);
            // not a valid target, return

            whereIsThisCreature.VisualState = VisualStates.Table;
            whereIsThisCreature.SetTableSortingOrder();
        }

        // return target and arrow to original position
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}

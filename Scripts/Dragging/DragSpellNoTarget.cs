using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private OneCardManager manager;

    public List<int> AvaliableSlots = new List<int>();
    public override bool CanDrag
    {
        get
        { 
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    void AssignStartingSlots()
    {
        if (playerOwner.ID == 1)
        {
            AvaliableSlots.Add(2);
            AvaliableSlots.Add(3);

        }

        if (playerOwner.ID == 2)
        {
            AvaliableSlots.Add(0);
            AvaliableSlots.Add(1);
        }
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        AssignStartingSlots();
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

       
       

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
      
        bool CreatureAlowwed;
        bool CorrectSlot = false;

        // determine table position
        int tablePos = playerOwner.PArea.DualTableVisual.NewTablePosForNewSpell(Camera.main.ScreenToWorldPoint(
             new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x, (Camera.main.ScreenToWorldPoint(
             new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).y));

        

        //Debug.LogWarning("tablePos : " + tablePos);

        for (int i = 0; i < AvaliableSlots.Count; i++)
        {
            if (tablePos == AvaliableSlots[i])
            {
                CorrectSlot = true;
            }
        }

      

        if (playerOwner.ID == 1 && CorrectSlot && Table.instance.ChechkIfSpellSlotIsFree(tablePos))
        {
            CreatureAlowwed = true;
        }
        else if (playerOwner.ID == 2 && CorrectSlot && Table.instance.ChechkIfSpellSlotIsFree(tablePos))
        {
            CreatureAlowwed = true;
        }
        else
        {
            CreatureAlowwed = false;
        }


        // 1) Check if we are holding a card over the table
        if (DragSuccessful() && CreatureAlowwed )
        {
            // play this card
            playerOwner.PlaySpellOnTable(GetComponent<IDHolder>().UniqueID, tablePos);
        }
        else
        {
            // Set old sorting order 
            whereIsCard.Slot = savedHandSlot;
            if (tag.Contains("Low"))
                whereIsCard.VisualState = VisualStates.LowHand;
            else
                whereIsCard.VisualState = VisualStates.TopHand;
            // Move this card back to its slot position
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {

        return TableVisual.CursorOverSomeTable; 
    }


}

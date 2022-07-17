using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class DragCreatureOnTable : DraggingActions
{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;

    int amountOfCards_Player1 = 1;
    int amountOfCards_Player2 = 1;
    int amountOfEmptySpaces = 0;
    bool TableNotFull_player1 = false;
    bool TableNotFull_player2 = false;
    bool AllowPlacment1 = false;
    bool AllowPlacment2 = false;

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
            for (int i = 5; i >= GlobalSettings.Instance.RowsAllowedForCreatures; i--)
            {
                AvaliableSlots.Add(i * 4);
                AvaliableSlots.Add(i * 4 + 1);
                AvaliableSlots.Add(i * 4 + 2);
                AvaliableSlots.Add(i * 4 + 3);
            }
        }

        if (playerOwner.ID == 2)
        {
            for (int i = 0; i < GlobalSettings.Instance.RowsAllowedForCreatures; i++)
            {
                AvaliableSlots.Add(i * 4);
                AvaliableSlots.Add(i * 4 + 1);
                AvaliableSlots.Add(i * 4 + 2);
                AvaliableSlots.Add(i * 4 + 3);
            }

        }
    }

    void CheckTable()
    {
        amountOfCards_Player1 = 0;
        amountOfCards_Player2 = 0;
        amountOfEmptySpaces = 0;
        TableNotFull_player1 = false;
        TableNotFull_player2 = false;
        AllowPlacment1 = false;
        AllowPlacment2 = false;


        for (int i = 0; i < Table.instance.UnitsOnTable.Count; i++)
        {

            if (Table.instance.ChechkIfUnitSlotIsFree(i))
            {
                amountOfEmptySpaces++;
            }
            else
            {

                if (Table.instance.UnitsOnTable[i].owner.tag.Contains("Low"))
                {
                    amountOfCards_Player1++;
                }

                else if (Table.instance.UnitsOnTable[i].owner.tag.Contains("Top"))
                {
                    amountOfCards_Player2++;
                }
            }


        }
        

        if (amountOfCards_Player1 < GlobalSettings.Instance.Number_of_cards_allowed_for_one_player)
        {
            TableNotFull_player1 = true;
        }
        if (amountOfCards_Player2 < GlobalSettings.Instance.Number_of_cards_allowed_for_one_player)
        {
            TableNotFull_player2 = true;
        }

        if (playerOwner.ID == 2 && TableNotFull_player1)
        {
            AllowPlacment1 = true;
        }

        else if (playerOwner.ID == 1 && TableNotFull_player2)
        {
            AllowPlacment2 = true;
        }
    }

    public override void OnStartDrag()
    {
        AssignStartingSlots();
        CheckTable();

        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

       

        if (playerOwner.ID == 1 && amountOfCards_Player2 < GlobalSettings.Instance.Number_of_cards_allowed_for_one_player)
        {
            TabeSlots.Instance.ChangeSlotColor(true, AvaliableSlots);
        }
        else if (playerOwner.ID == 1 && amountOfCards_Player2 >= GlobalSettings.Instance.Number_of_cards_allowed_for_one_player)
        {
            TabeSlots.Instance.ChangeSlotColor(false, AvaliableSlots);
        }

        else if (playerOwner.ID == 2 && amountOfCards_Player1 < GlobalSettings.Instance.Number_of_cards_allowed_for_one_player)
        {
            TabeSlots.Instance.ChangeSlotColor(true, AvaliableSlots);
        }
        else if (playerOwner.ID == 2 && amountOfCards_Player1 >= GlobalSettings.Instance.Number_of_cards_allowed_for_one_player)
        {
            TabeSlots.Instance.ChangeSlotColor(false, AvaliableSlots);
        }

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        TabeSlots.Instance.ReturnDefaultSlotColor(AvaliableSlots);
        bool CreatureAlowwed;
        bool CorrectSlot = false;


        int tablePos = playerOwner.PArea.DualTableVisual.TablePositionForNewUnit
            (Camera.main.ScreenToWorldPoint(
                 new Vector3(Input.mousePosition.x,
                             Input.mousePosition.y, 
                             transform.position.z - Camera.main.transform.position.z)).x,
             (Camera.main.ScreenToWorldPoint(
                 new Vector3(Input.mousePosition.x,
                             Input.mousePosition.y,
                             transform.position.z - Camera.main.transform.position.z)).y));


        for (int i = 0; i < AvaliableSlots.Count; i++)
        {
            if (tablePos == AvaliableSlots[i])
            {
                CorrectSlot = true;
            }
        }


        if (playerOwner.ID == 1 && CorrectSlot && Table.instance.ChechkIfUnitSlotIsFree(tablePos))
        {
            CreatureAlowwed = true;
        }
        else if (playerOwner.ID == 2 && CorrectSlot && Table.instance.ChechkIfUnitSlotIsFree(tablePos))
        {
            CreatureAlowwed = true;
        }
        else
        {

            CreatureAlowwed = false;
        }





        if (DragSuccessful() && CreatureAlowwed && (AllowPlacment1 ^ AllowPlacment2))
        {

            playerOwner.PlayUnitFromHand(GetComponent<IDHolder>().UniqueID, tablePos);



        }
        else
        {
            
          
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = tempState;
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

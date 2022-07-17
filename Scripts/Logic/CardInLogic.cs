using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardInLogic: I_ID
{
    // reference to a player who holds this card in his hand
    public Player owner;
    // an ID of this card
    public int UniqueCardID; 
    // a reference to the card asset that stores all the info about this card
    public CardAsset _cardAsset;

    public SpellEffect effect;
    public TableSpellEffect tab_effect;
   
    public HeroAsset _heroAsset;
   
    public static Dictionary<int, CardInLogic> CardsCreatedThisGame = new Dictionary<int, CardInLogic>();
    public static List<CardInLogic> AllCards = new List<CardInLogic>();

    public static CardInLogic FindCardLogicByID(int ID)
    {
        foreach (CardInLogic cl in AllCards)
        {
            if (cl.ID == ID)
            {
                return cl;
            }
        }
        return null;
    }

    public int ID
    {
        get{ return UniqueCardID; }
    }

    public int CurrentAPCost{ get; set; }

    public bool CanBePlayed
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
                   
            return ownersTurn  && (CurrentAPCost <= owner.ApLeft);
        }
    }

    public CardInLogic(CardAsset ca)
    {
        _cardAsset = ca;
        UniqueCardID = IDFactory.GetUniqueID();
        SetAPCost();
   
        if (ca.MaxHealth == 0)
        {
            if (ca.SpellScriptName != null && ca.SpellScriptName != "")
            {
                if (ca.Targets == TargetingOptions.NoTarget)
                {
                   // tab_effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as TableSpellEffect;

                }
                else
                {
                    effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
                }

            }
        }
       
        CardsCreatedThisGame.Add(UniqueCardID, this);
        AllCards.Add(this);
    }

    public CardInLogic(HeroAsset ha)
    {   
        _heroAsset = ha;
        UniqueCardID = IDFactory.GetUniqueID();

        CardsCreatedThisGame.Add(UniqueCardID, this);
        AllCards.Add(this);
    }

    public void SetAPCost()
    {
        CurrentAPCost = _cardAsset.AP_Cost;
    }

   


}

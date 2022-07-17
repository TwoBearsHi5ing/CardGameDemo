﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNameRibbon : MonoBehaviour {

    public Text NameText;
    public Text QuantityText;
    public Image RibbonImage;

    public CardAsset Asset{get; set;}
    public int Quantity{ get; set;}

    public void ApplyAsset(CardAsset ca, int quantity)
    {
        if (ca.factionAsset != null)
        {
            RibbonImage.color = ca.factionAsset.FactionRibbonsTint;

        }
        

        Asset = ca;

        NameText.text = ca.name;
        SetQuantity(quantity);
    }

    public void SetQuantity(int quantity)
    {
        if (quantity == 0)
            return;
        
        QuantityText.text ="X" + quantity.ToString();
        Quantity = quantity;
    }

    public void ReduceQuantity()
    {   
        //Debug.Log("In reduce Quantity");
        DeckBuildingScreen.Instance.BuilderScript.RemoveCard(Asset);
    }
}
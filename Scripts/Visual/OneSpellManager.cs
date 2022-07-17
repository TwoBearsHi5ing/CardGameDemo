using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneSpellManager : MonoBehaviour
{
    public CardAsset cardAsset;
    public OneCardManager PrevierManager;
    public OneSpellManager HighlightManager;

    [Header("Text Component References")]
    public Text NumberOfTurns;
   
    [Header("Image References")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;
    public Image CreatureFaction;
    public Image CreatureBackgound;

    void Awake()
    {
        if (cardAsset != null)
            ReadCreatureFromAsset();
    }



    public void updateStats(int TurnsLeft)
    {
        NumberOfTurns.text = TurnsLeft.ToString();
      
        HighlightManager.NumberOfTurns.text = TurnsLeft.ToString();
     
    }

    public void ReadCreatureFromAsset()
    {
        CreatureGraphicImage.sprite = cardAsset.CardImage;

        NumberOfTurns.text = cardAsset.numberOfturns.ToString();
        

        if (cardAsset.factionAsset != null)
        {
            CreatureBackgound.color = cardAsset.factionAsset.FactionBackgroundColor;
            CreatureFaction.sprite = cardAsset.factionAsset.FactionImage;
        }


        if (PrevierManager != null && HighlightManager != null)
        {
            PrevierManager.cardAsset = cardAsset;
            PrevierManager.ReadCardFromAsset(false);
            HighlightManager.cardAsset = cardAsset;
            HighlightManager.ReadCreatureFromAsset();

        }
    }

  
}

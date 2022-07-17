using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text APCostText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;

   // [Header ("GameObject References")]
  //  public GameObject HealthIcon;
  //  public GameObject AttackIcon;
    [Header("Image References")]
    public Image CardTopRibbonImage;
    public Image CardLowRibbonImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;
    public Image CardBackGlowImage;
    public Image CardFactionImage;
    public Image CardBackground;

    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset(false);
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void ReadCardFromAsset(bool menu)
    {
        // universal actions for any Card
        // 1) apply tint
        if (cardAsset.factionAsset != null)
        {
            CardBodyImage.color = cardAsset.factionAsset.FactionCardBodyTint;
            CardTopRibbonImage.color = cardAsset.factionAsset.FactionRibbonsTint;
            CardLowRibbonImage.color = cardAsset.factionAsset.FactionRibbonsTint;
            CardFactionImage.sprite = cardAsset.factionAsset.FactionImage;
            CardBackground.color = cardAsset.factionAsset.FactionBackgroundColor;
        }
        else
        {
            CardFaceFrameImage.color = Color.white;
          
            if (!menu)
            {
                CardFactionImage.sprite = GlobalSettings.Instance.DefaultFactionImage;
            }
       
        }

        NameText.text = cardAsset.name;   
        APCostText.text = cardAsset.AP_Cost.ToString();
        DescriptionText.text = cardAsset.Description;
        CardGraphicImage.sprite = cardAsset.CardImage;

        if (cardAsset.TypeOfCard == TypesOfCards.Unit)
        {
            AttackText.text = cardAsset.Attack.ToString();
            HealthText.text = cardAsset.MaxHealth.ToString();
        }
        else
        {
          
        }

        if (PreviewManager != null)
        {
           
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset(false);
        }
    }
}

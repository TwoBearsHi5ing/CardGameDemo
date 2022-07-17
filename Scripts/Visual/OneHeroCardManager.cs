using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneHeroCardManager : MonoBehaviour
{
    
    public HeroAsset heroAsset;
    public OneHeroCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;

 
    [Header("Image References")]
    public Image CardTopRibbonImage;
    public Image CardLowRibbonImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;
    public Image CardBackGlowImage;
    public Image CardFactionImage;
    public Image CardBackgroudImage;

    void Awake()
    {
        if (heroAsset != null)
            ReadCardFromAsset();
    }



    public void ReadCardFromAsset()
    {
        // universal actions for any Card
        // 1) apply tint
        if (heroAsset.faction != null)
        {
            CardBodyImage.color = heroAsset.faction.FactionCardBodyTint;
            // CardFaceFrameImage.color = heroAsset.faction.FactionCardTint;
            CardTopRibbonImage.color = heroAsset.faction.FactionRibbonsTint;
            CardLowRibbonImage.color = heroAsset.faction.FactionRibbonsTint;
            CardFactionImage.sprite = heroAsset.faction.FactionImage;
            CardBackgroudImage.color = heroAsset.faction.FactionBackgroundColor;
        }
        else
        {
            CardBodyImage.color = GlobalSettings.Instance.CardBodyStandardColor;
            //CardFaceFrameImage.color = Color.white;
            CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            CardLowRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            CardFactionImage.sprite = GlobalSettings.Instance.DefaultFactionImage;
            CardBackgroudImage.color = GlobalSettings.Instance.DefaultBackgroundColor;

           
        }

        NameText.text = heroAsset.name;
        DescriptionText.text = heroAsset.Description;
     
        CardGraphicImage.sprite = heroAsset.HeroImage;
      
          
         AttackText.text = heroAsset.Attack.ToString();
         HealthText.text = heroAsset.MaxHealth.ToString();
       
        

        if (PreviewManager != null)
        {
            
            PreviewManager.heroAsset = heroAsset;
        }
    }
}

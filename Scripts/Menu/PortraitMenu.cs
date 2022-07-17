using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PortraitMenu : MonoBehaviour {

    public HeroAsset asset;
    public Image FactionBackground;
    private OneHeroCardManager portrait;
   
    private bool selected = false;
    

    void Awake()
    {
        portrait = GetComponent<OneHeroCardManager>();
        portrait.heroAsset = asset;
        portrait.ReadCardFromAsset();
        FactionBackground.sprite = portrait.heroAsset.faction.FactionImage;
    }

    public void Deselect()
    {
        selected = false;
    }
    public void chooseHero()
    {
        if (!selected)
        {
            selected = true;
            CharacterSelectionScreen.Instance.HeroPanel.SelectCharacter(this);
            // deselect all the other Portrait Menu buttons 
            PortraitMenu[] allPortraitButtons = GameObject.FindObjectsOfType<PortraitMenu>();
            foreach (PortraitMenu m in allPortraitButtons)
                if (m != this)
                    m.Deselect();
        }
        else
        {
            Deselect();
            CharacterSelectionScreen.Instance.HeroPanel.SelectCharacter(null);
        }
    }
}

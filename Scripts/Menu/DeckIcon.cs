using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DeckIcon : MonoBehaviour 
{
    public bool AIDeckChoice;
    public Text DeckNameText; 
    public GameObject DeckNotCompleteObject;
    public Image heroImage;
    public Image factionImage;
    public Image Checkmark;
    private float InitialScale;
    private float TargetScale = 1.3f;
    private bool selected = false;
    private bool AiSelected = false;

    public DeckInfo DeckInformation { get; set;}

    public void ApplyLookToIcon(DeckInfo info)
    {
        DeckInformation = info;
        Checkmark.enabled = false;
        DeckNotCompleteObject.SetActive(!info.IsComplete());

        factionImage.sprite = info.Faction.FactionImage;
        heroImage.sprite = info.heroAsset.HeroImage;
        DeckNameText.text = info.DeckName;
    }

    void OnMouseDown()
    {
       
        
        if (!selected)
        {
            selected = true;
            // zoom in on the deck only if it is complete
            //if (DeckInformation.IsComplete())
               // transform.DOScale(TargetScale, 0.5f);

            DeckSelectionScreen.Instance.HeroPanelDeckSelection.SelectDeck(this);
            // deselect all the other Portrait Menu buttons 
            DeckIcon[] allPortraitButtons = GameObject.FindObjectsOfType<DeckIcon>();
            foreach (DeckIcon m in allPortraitButtons)
                if (m != this)
                    m.Deselect();
        }
        else
        {
            Deselect();
            DeckSelectionScreen.Instance.HeroPanelDeckSelection.SelectDeck(null);
        }
        
    }

    public void Deselect()
    {
        //transform.DOScale(InitialScale, 0.5f);
        selected = false;
        Checkmark.enabled = false;
    }

    public void InstantDeselect()
    {
        //transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        selected = false;
        Checkmark.enabled = false;
    }

    public void Select()
    {
        if (!selected)
        {
            Checkmark.enabled = true;
            selected = true;
            // zoom in on the deck only if it is complete
            //if (DeckInformation.IsComplete())
                // transform.DOScale(TargetScale, 0.5f);

                DeckSelectionScreen.Instance.HeroPanelDeckSelection.SelectDeck(this);
            // deselect all the other Portrait Menu buttons 
            DeckIcon[] allPortraitButtons = GameObject.FindObjectsOfType<DeckIcon>();
            foreach (DeckIcon m in allPortraitButtons)
                if (m != this)
                    m.Deselect();
        }
        else
        {
            Deselect();
         
            DeckSelectionScreen.Instance.HeroPanelDeckSelection.SelectDeck(null);
        }
    }


    public void AIDeselect()
    {
        //transform.DOScale(InitialScale, 0.5f);
        AiSelected = false;
        Checkmark.enabled = false;
    }

    public void AiInstantDeselect()
    {
        //transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        AiSelected = false;
        Checkmark.enabled = false;
    }

    public void AISelect()
    {
        if (!AiSelected)
        {
            AiSelected = true;
            Checkmark.enabled = true;
            // zoom in on the deck only if it is complete
            //if (DeckInformation.IsComplete())
                // transform.DOScale(TargetScale, 0.5f);

                AIDeckSelectionScreen.Instance.HeroPanelDeckSelection.AISelectDeck(this);
            // deselect all the other Portrait Menu buttons 
            DeckIcon[] allPortraitButtons = GameObject.FindObjectsOfType<DeckIcon>();
            foreach (DeckIcon m in allPortraitButtons)
                if (m != this)
                    m.AIDeselect();
        }
        else
        {
            AIDeselect();
            AIDeckSelectionScreen.Instance.HeroPanelDeckSelection.AISelectDeck(null);
        }
    }


}

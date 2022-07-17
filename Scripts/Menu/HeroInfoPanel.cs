using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoPanel : MonoBehaviour
{

    public bool AIdeck;
    public Button PlayButton;
    public PortraitMenu selectedPortrait { get; set; }
    public DeckIcon selectedDeck { get; set; }
    public DeckIcon AIselectedDeck { get; set; }

    void Awake()
    {
        OnOpen();
    }

    public void OnOpen()
    {
        SelectCharacter(null);
        if (AIdeck)
        {
            AISelectDeck(null);
        }
        else
        {
            SelectDeck(null);
        }


    }

    public void SelectCharacter(PortraitMenu menuPortrait)
    {
        if (menuPortrait == null || selectedPortrait == menuPortrait)
        {
            selectedPortrait = null;
        }
        else
        {      
            selectedPortrait = menuPortrait;
            GoToDeckbuilding();
        }
    }

    public void SelectDeck(DeckIcon deck)
    {

        if (deck == null || selectedDeck == deck || !deck.DeckInformation.IsComplete())
        {
            selectedDeck = null;
            if (PlayButton != null)
                PlayButton.interactable = false;
        }
        else
        {
            selectedDeck = deck;
            BattleStartInfo.SelectedDeck = selectedDeck.DeckInformation;

            if (PlayButton != null)
                PlayButton.interactable = true;
        }


    }

    public void AISelectDeck(DeckIcon deck)
    {

        if (deck == null || AIselectedDeck == deck || !deck.DeckInformation.IsComplete())
        {
            AIselectedDeck = null;
            if (PlayButton != null)
                PlayButton.interactable = false;
        }
        else
        {
            AIselectedDeck = deck;
            // instantly load this information to our BattleStartInfo.
            BattleStartInfo.EmenyDeck = AIselectedDeck.DeckInformation;

            if (PlayButton != null)
                PlayButton.interactable = true;
        }


    }

    // this method is called when we are on the character selection screen
    // it opens the deck bulder for the character that we have selected
    public void GoToDeckbuilding()
    {
        if (selectedPortrait == null)
            return;

        DeckBuildingScreen.Instance.BuildADeckFor(selectedPortrait.asset);
    }

}

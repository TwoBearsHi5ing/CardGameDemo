using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionBrowser : MonoBehaviour {

    public Transform[] Slots;
    public GameObject SpellMenuPrefab;
    public GameObject CreatureMenuPrefab;

    public GameObject OneCharacterTabs;
    public GameObject AllCharactersTabs;

    public KeywordInputField KeywordInputFieldScript;
    public CardsThatYouDoNotHaveToggle CardsThatYouDoNotHaveToggleScript;
    public ManaFilter ManaFilterSctipt;

    private FactionAsset _character;

    private List<GameObject> CreatedCards = new List<GameObject>();
   
    #region PROPERTIES
    private bool _showingCardsPlayerDoesNotOwn = false;
    public bool ShowingCardsPlayerDoesNotOwn
    {
        get { return _showingCardsPlayerDoesNotOwn;}

        set
        {
            _showingCardsPlayerDoesNotOwn = value;
            UpdatePage();
        }
    }

    private int _pageIndex = 0;
    public int PageIndex 
    {
        get{ return _pageIndex;}
        set
        {
            _pageIndex = value;
            UpdatePage();
        }
    }

    private bool _includeAllRarities = true;
    public bool IncludeAllRarities
    {
        get{ return _includeAllRarities; }
        set
        {
            _includeAllRarities = value;
            UpdatePage();
        }
    }

    private bool _includeAllCharacters = true; 
    public bool IncludeAllCharacters
    {
        get{ return _includeAllCharacters; }
        set
        {
            _includeAllCharacters = value;
            _pageIndex = 0;

            UpdatePage();
        }
    }

    private RarityOptions _rarity = RarityOptions.Basic;
    public RarityOptions Rarity 
    {
        get{ return _rarity; }
        set
        {
            _rarity = value;
            UpdatePage();
        }
    }

    private FactionAsset _asset = null; 
    public FactionAsset Asset
    {
        get{ return _asset; }
        set
        {
            _asset = value;
            _pageIndex = 0;
            UpdatePage();
        }
    }

    private string _keyword = "";
    public string Keyword
    {
        get{ return _keyword;}
        set
        {
            _keyword = value;
            UpdatePage();
        }
    }

    private int _manaCost = -1; 
    public int ManaCost
    {
        get{ return _manaCost; }
        set
        {
            _manaCost = value;
            _pageIndex = 0;
            UpdatePage();
        }
    }

    private bool _includeTokenCards = false; 
    public bool IncludeTokenCards
    {
        get{ return _includeTokenCards; }
        set
        {
            _includeTokenCards = value;
            UpdatePage();
        }
    }
    #endregion

    public void ShowCollectionForBrowsing()
    {
        // reset keyword input field, reset toggle, reset mana filter:
        KeywordInputFieldScript.Clear();
        CardsThatYouDoNotHaveToggleScript.SetValue(true);
        ManaFilterSctipt.RemoveAllFilters();

       // Debug.LogWarning("Collection browser !");

        ShowCards(false, 0, true, false, RarityOptions.Basic, null, "", -1, false);

        // select neutral tab by default:
        DeckBuildingScreen.Instance.TabsScript.NeutralTabWhenCollectionBrowsing.Select(instant: true);   
        DeckBuildingScreen.Instance.TabsScript.SelectTab(DeckBuildingScreen.Instance.TabsScript.NeutralTabWhenCollectionBrowsing, instant: true);
    }

    public void ShowCollectionForDeckBuilding(FactionAsset buildingForThisCharacter)
    {
        // reset keyword input field, reset toggle, reset mana filter:
        KeywordInputFieldScript.Clear();
        CardsThatYouDoNotHaveToggleScript.SetValue(true);
        ManaFilterSctipt.RemoveAllFilters();

        _character = buildingForThisCharacter;

        ShowCards(false, 0, true, false, RarityOptions.Basic, _character, "", -1, false);

        // select a tab with class cards by default
        DeckBuildingScreen.Instance.TabsScript.ClassTab.Select(instant: true);
        DeckBuildingScreen.Instance.TabsScript.SelectTab(DeckBuildingScreen.Instance.TabsScript.ClassTab, instant: true);
    }

    public void ClearCreatedCards()
    {
        while(CreatedCards.Count>0)
        {
            GameObject g = CreatedCards[0];
            CreatedCards.RemoveAt(0);
            Destroy(g);
        }
    }

    public void UpdateQuantitiesOnPage()
    {
        foreach (GameObject card in CreatedCards)
        {
            AddCardToDeck addCardComponent = card.GetComponent<AddCardToDeck>();
            addCardComponent.UpdateQuantity();
        }
    }

    // a method to display cards based on all the selected parameters
    public void UpdatePage()
    {
       // Debug.LogWarning("Collection browser - Update Page!");

        ShowCards(_showingCardsPlayerDoesNotOwn, _pageIndex, _includeAllRarities, _includeAllCharacters, _rarity, _asset, _keyword, _manaCost, _includeTokenCards);
       // Debug.LogWarning("Collection browser - Update Page! 2");
    }

    private void ShowCards(bool showingCardsPlayerDoesNotOwn = false, int pageIndex = 0, bool includeAllRarities = true, bool includeAllCharacters = true, 
        RarityOptions rarity = RarityOptions.Basic, FactionAsset asset = null, string keyword = "", int manaCost = -1, bool includeTokenCards = false)
    {
        // saving the information about the cards that we are showing to players on this page
        _showingCardsPlayerDoesNotOwn = showingCardsPlayerDoesNotOwn;
        _pageIndex = pageIndex;
        _includeAllRarities = includeAllRarities;
        _includeAllCharacters = includeAllCharacters;
        _rarity = rarity;
        _asset = asset;
        _keyword = keyword;
        _manaCost = manaCost;
        _includeTokenCards = includeTokenCards;
        
        List<CardAsset> CardsOnThisPage = PageSelection(showingCardsPlayerDoesNotOwn, pageIndex, includeAllRarities, includeAllCharacters, rarity,
            asset, keyword, manaCost, includeTokenCards);

        // clear created cards list 
        ClearCreatedCards();

        if (CardsOnThisPage.Count == 0)
            return;
        
        // Debug.Log(CardsOnThisPage.Count);

        for (int i = 0; i < CardsOnThisPage.Count; i++)
        {

            GameObject newMenuCard = null;

            if (CardsOnThisPage[i].TypeOfCard == TypesOfCards.Unit)
            {
                // it is a creature card
                newMenuCard = Instantiate(CreatureMenuPrefab, Slots[i].position, Quaternion.identity) as GameObject;
            }
            else if (CardsOnThisPage[i].TypeOfCard == TypesOfCards.Spell)
            {
                // it is a spell card
                newMenuCard = Instantiate(SpellMenuPrefab, Slots[i].position, Quaternion.identity) as GameObject;
            }
            if (newMenuCard != null)
            {
                newMenuCard.transform.SetParent(this.transform);

                CreatedCards.Add(newMenuCard);

                OneCardManager manager = newMenuCard.GetComponent<OneCardManager>();
                manager.cardAsset = CardsOnThisPage[i];
                manager.ReadCardFromAsset(true);

                AddCardToDeck addCardComponent = newMenuCard.GetComponent<AddCardToDeck>();
                addCardComponent.SetCardAsset(CardsOnThisPage[i]);
                addCardComponent.UpdateQuantity();
               
            }
        }

    }

    public void Next()
    {
        if (PageSelection(_showingCardsPlayerDoesNotOwn, _pageIndex+1,_includeAllRarities, _includeAllCharacters, _rarity,
            _asset,_keyword,_manaCost, _includeTokenCards).Count == 0)
            return;
        
        ShowCards(_showingCardsPlayerDoesNotOwn, _pageIndex+1,_includeAllRarities, _includeAllCharacters, _rarity,
            _asset,_keyword,_manaCost, _includeTokenCards);
    }

    public void Previous()
    {
        if (_pageIndex == 0)
            return;

        ShowCards(_showingCardsPlayerDoesNotOwn, _pageIndex-1, _includeAllRarities, _includeAllCharacters, _rarity,
            _asset, _keyword, _manaCost, _includeTokenCards);
    }

    // Returns a list with assets of cards that we have to show on page with pageIndex that. Selects cards that satisfy all the other parameters (rarity, manaCost, etc...)
    private List<CardAsset> PageSelection(bool showingCardsPlayerDoesNotOwn = false, int pageIndex = 0, bool includeAllRarities = true, bool includeAllCharacters = true, 
        RarityOptions rarity = RarityOptions.Basic, FactionAsset asset = null, string keyword = "", int manaCost = -1, bool includeTokenCards = false)
    {
        List<CardAsset> returnList = new List<CardAsset>();

        // obtain cards from collection that satisfy all the selected criteria
        List<CardAsset> cardsToChooseFrom = CardCollection.Instance.GetCards(showingCardsPlayerDoesNotOwn, includeAllRarities, includeAllCharacters, rarity,
            asset, keyword, manaCost);

        if (cardsToChooseFrom.Count > pageIndex * Slots.Length)
        {      
            for (int i = 0; (i < cardsToChooseFrom.Count - pageIndex * Slots.Length && i < Slots.Length); i++)
            {
                returnList.Add(cardsToChooseFrom[pageIndex * Slots.Length + i]);
            }
        }

        return returnList;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckInfo
{
    public string DeckName;
    public FactionAsset Faction;
    public HeroAsset heroAsset;
    public List<CardAsset> Cards;

    public DeckInfo(List<CardAsset> cards, string deckName, FactionAsset faction, HeroAsset hero)
    {
        Cards = new List<CardAsset>(cards);
        heroAsset = hero;
        Faction = faction;       
        DeckName = deckName;
    }

    public bool IsComplete()
    {
        return (DeckBuildingScreen.Instance.BuilderScript.AmountOfCardsInDeck == Cards.Count);
    }

    public int NumberOfThisCardInDeck (CardAsset asset)
    {
        int count = 0;
        foreach (CardAsset ca in Cards)
        {
            if (ca == asset)
                count++;
        }
        return count;
    }
}

public class DecksStorage : MonoBehaviour {

    public static DecksStorage Instance;
    public List<DeckInfo> AllDecks { get; set;}  

    private bool alreadyLoadedDecks = false;

    void Awake()
    {
        AllDecks = new List<DeckInfo>();
        Instance = this;
    }

    void Start()
    {
        if (!alreadyLoadedDecks)
        {
            LoadDecksFromPlayerPrefs();
            alreadyLoadedDecks = true;
        }
    }

    void LoadDecksFromPlayerPrefs()
    {
        List<DeckInfo> DecksFound = new List<DeckInfo>();
        // load the information about decks from PlayerPrefsX
        for(int i=0; i < 7; i++)
        {
            string deckListKey = "Deck" + i.ToString();
            string factionKey = "DeckFaction" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();
            string heroKey = "DeckHero" + i.ToString();
            string[] DeckAsCardNames = PlayerPrefsX.GetStringArray(deckListKey);

            if (DeckAsCardNames.Length > 0 && PlayerPrefs.HasKey(factionKey) && PlayerPrefs.HasKey(deckNameKey) && PlayerPrefs.HasKey(heroKey))
            {
                string factionName = PlayerPrefs.GetString(factionKey);
                string deckName = PlayerPrefs.GetString(deckNameKey);
                string heroName = PlayerPrefs.GetString(heroKey);

                // make a CardAsset list from an array of strings:
                List <CardAsset> deckList = new List<CardAsset>();
                foreach(string name in DeckAsCardNames)
                {
                    deckList.Add(CardCollection.Instance.GetCardAssetByName(name));
                }

                DecksFound.Add(new DeckInfo(deckList, deckName, FactionAssetsByName.Instance.GetCharacterByName(factionName) ,HeroAssetByName.Instance.GetCharacterByName(heroName)));
            }
        }

        AllDecks = DecksFound;
    }

    public void SaveDecksIntoPlayerPrefs()
    {
        // clear all the keys of characters and deck names
        for(int i=0; i < 7; i++)
        {
            string factionKey = "DeckFaction" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();
            string heroKey = "DeckHero" + i.ToString();
           
            if (PlayerPrefs.HasKey(factionKey))
            {
                PlayerPrefs.DeleteKey(factionKey);
            }

            if(PlayerPrefs.HasKey(deckNameKey))
            {
                PlayerPrefs.DeleteKey(deckNameKey);
            }

            if (PlayerPrefs.HasKey(heroKey))
            {
                PlayerPrefs.DeleteKey(heroKey);
            }
        }

        for(int i=0; i< AllDecks.Count; i++)
        {
            string deckListKey = "Deck" + i.ToString();
            string factionKey = "DeckFaction" + i.ToString();
            string deckNameKey = "DeckName" + i.ToString();
            string heroKey = "DeckHero" + i.ToString();

            List<string> cardNamesList = new List<string>();
            foreach (CardAsset a in AllDecks[i].Cards)
                cardNamesList.Add(a.name);

            string[] cardNamesArray = cardNamesList.ToArray();

            PlayerPrefsX.SetStringArray(deckListKey, cardNamesArray);
            PlayerPrefs.SetString(deckNameKey, AllDecks[i].DeckName);
            PlayerPrefs.SetString(factionKey, AllDecks[i].Faction.name);
            PlayerPrefs.SetString(heroKey, AllDecks[i].heroAsset.name);
        }
    }

    void OnApplicationQuit()
    {
        SaveDecksIntoPlayerPrefs();    
    }
}

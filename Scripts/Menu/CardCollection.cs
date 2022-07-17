using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardCollection : MonoBehaviour 
{
    public int DefaultNumberOfBasicCards = 3; 

    public static CardCollection Instance;
    private Dictionary<string, CardAsset > AllCardsDictionary = new Dictionary<string, CardAsset>();

    public Dictionary<CardAsset, int> QuantityOfEachCard = new Dictionary<CardAsset, int>();

    private CardAsset[] allCardsArray;

    void Awake()
    {
        Instance = this;

        allCardsArray = Resources.LoadAll<CardAsset>("");
   
        foreach (CardAsset ca in allCardsArray)
        {
            if (!AllCardsDictionary.ContainsKey(ca.name))
                AllCardsDictionary.Add(ca.name, ca);
        }      

        LoadQuantityOfCardsFromPlayerPrefs();
    }

    private void LoadQuantityOfCardsFromPlayerPrefs()
    {
       

        foreach (CardAsset ca in allCardsArray)
        {
            
            if(ca.Rarity == RarityOptions.Basic)
                QuantityOfEachCard.Add(ca, DefaultNumberOfBasicCards);            
            else if (PlayerPrefs.HasKey("NumberOf" + ca.name))
                QuantityOfEachCard.Add(ca, PlayerPrefs.GetInt("NumberOf" + ca.name));
            else
                QuantityOfEachCard.Add(ca, 0);
        }
    }

    private void SaveQuantityOfCardsIntoPlayerPrefs()
    {
        foreach (CardAsset ca in allCardsArray)
        {
            if (ca.Rarity == RarityOptions.Basic)
                PlayerPrefs.SetInt("NumberOf" + ca.name, DefaultNumberOfBasicCards);
            else
                PlayerPrefs.SetInt("NumberOf" + ca.name, QuantityOfEachCard[ca]);
        }
    }

    void OnApplicationQuit()
    {
        SaveQuantityOfCardsIntoPlayerPrefs();
    }

    public CardAsset GetCardAssetByName(string name)
    {        
        if (AllCardsDictionary.ContainsKey(name))  // if there is a card with this name, return its CardAsset
            return AllCardsDictionary[name];
        else        // if there is no card with name
            return null;
    }	

    public List<CardAsset> GetCardsOfCharacter(FactionAsset asset)
    {   
        return GetCards(true, true, false, RarityOptions.Basic, asset);
    }

    public List<CardAsset> GetCardsWithRarity(RarityOptions rarity)
    {
        return GetCards(true, false, true, rarity);
    }

    /// the most general method that will use multiple filters
    public List<CardAsset> GetCards(bool showingCardsPlayerDoesNotOwn = false, bool includeAllRarities = true, bool includeAllCharacters = true, RarityOptions rarity = RarityOptions.Basic,
                FactionAsset asset = null, string keyword = "", int manaCost = -1)
    {
        // initially select all cards
        var cards = from card in allCardsArray select card;

        if (!showingCardsPlayerDoesNotOwn)
            cards = cards.Where(card => QuantityOfEachCard[card] > 0);

        if (!includeAllRarities)
            cards = cards.Where(card => card.Rarity == rarity);

        if (!includeAllCharacters)
            cards = cards.Where(card => card.factionAsset == asset);

        if (keyword != null && keyword != "")
            cards = cards.Where(card => (card.name.ToLower().Contains(keyword.ToLower()) || 
                (card.Tags.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(" "))));

        if (manaCost == 7)
            cards = cards.Where(card => card.AP_Cost >= 7);
        else if (manaCost != -1)
            cards = cards.Where(card => card.AP_Cost == manaCost);                
        
        var returnList = cards.ToList<CardAsset>();
        returnList.Sort();
        return returnList;
    }
}

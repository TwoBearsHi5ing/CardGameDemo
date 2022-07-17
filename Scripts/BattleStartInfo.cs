using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStartInfo
{
    public List<CardAsset> deckList;
    public HeroAsset playerClass;
}

public static class BattleStartInfo  
{
    public static PlayerStartInfo[] PlayerInfos;
    public static DeckInfo SelectedDeck;
    public static DeckInfo EmenyDeck;
}

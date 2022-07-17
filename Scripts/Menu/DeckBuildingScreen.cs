using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildingScreen : MonoBehaviour {

    public GameObject ScreenContent;
    public GameObject ReadyDecksList;
    public GameObject CardsInDeckList;
    public DeckBuilder BuilderScript;
    public ListOfDecksInCollection ListOfReadyMadeDecksScript;
    public CollectionBrowser CollectionBrowserScript;
    public FactionSelectionTabs TabsScript;
    public bool ShowReducedQuantitiesInDeckBuilding = true;

    public static DeckBuildingScreen Instance;

    void Awake () 
    {
        Instance = this;    
        HideScreen();
    }   

    public void ShowScreenForCollectionBrowsing()
    {
        ScreenContent.SetActive(true);
        ReadyDecksList.SetActive(true);
        CardsInDeckList.SetActive(false);
        BuilderScript.InDeckBuildingMode = false;
        ListOfReadyMadeDecksScript.UpdateList();

        CollectionBrowserScript.AllCharactersTabs.gameObject.SetActive(true);
        CollectionBrowserScript.OneCharacterTabs.gameObject.SetActive(false);
        Canvas.ForceUpdateCanvases();

        CollectionBrowserScript.ShowCollectionForBrowsing();
    } 

    public void ShowScreenForDeckBuilding()
    {
        ScreenContent.SetActive(true);
        ReadyDecksList.SetActive(false);
        CardsInDeckList.SetActive(true);

        CollectionBrowserScript.AllCharactersTabs.gameObject.SetActive(false);
        CollectionBrowserScript.OneCharacterTabs.gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
    }

    public void BuildADeckFor(HeroAsset asset)
    {
        ShowScreenForDeckBuilding();
        CollectionBrowserScript.ShowCollectionForDeckBuilding(asset.faction);
        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(asset.faction);
        BuilderScript.BuildADeckFor(asset);
    }

    public void HideScreen()
    {
        ScreenContent.SetActive(false);
        CollectionBrowserScript.ClearCreatedCards();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DeckInScrollList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Image AvatarImage;
    public Text NameText;
    public GameObject DeleteDeckButton;
    public DeckInfo savedDeckInfo;

    public void Awake()
    {
        DeleteDeckButton.SetActive(false);
    }

    public void EditThisDeck()
    {
        DeckBuildingScreen.Instance.HideScreen();
        DeckBuildingScreen.Instance.BuilderScript.BuildADeckFor(savedDeckInfo.heroAsset);
        DeckBuildingScreen.Instance.BuilderScript.DeckName.text = savedDeckInfo.DeckName;

        foreach (CardAsset asset in savedDeckInfo.Cards)
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);

        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(savedDeckInfo.heroAsset.faction);
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowCollectionForDeckBuilding(savedDeckInfo.heroAsset.faction);
        DeckBuildingScreen.Instance.ShowScreenForDeckBuilding();
    }

    public void DeleteThisDeck()
    {
        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
        Destroy(gameObject);
    }

    public void ApplyInfo (DeckInfo deckInfo)
    {
        AvatarImage.sprite = deckInfo.Faction.FactionImage;
        NameText.text = deckInfo.DeckName;
        savedDeckInfo = deckInfo;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        // show delete deck button
        DeleteDeckButton.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        // hide delete deck button
        DeleteDeckButton.SetActive(false);
    }
}

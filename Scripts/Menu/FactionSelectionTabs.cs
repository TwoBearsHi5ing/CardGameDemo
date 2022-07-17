using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionSelectionTabs : MonoBehaviour 
{
    public List<FactionFilterTab> Tabs = new List<FactionFilterTab>();
    public FactionFilterTab ClassTab;
    public FactionFilterTab NeutralTabWhenCollectionBrowsing;
    private int currentIndex = 0;

    public void SelectTab(FactionFilterTab tab, bool instant)
    {
        int newIndex = Tabs.IndexOf(tab);

        if (newIndex == currentIndex)
            return;

        currentIndex = newIndex;

        // we have selected a new tab
        // remove highlights from all the other tabs:
        foreach (FactionFilterTab t in Tabs)
        {
            if (t != tab)
                t.Deselect(instant);
        }
        // select the tab that we have picked
        tab.Select(instant);
 
        // update the cards in the collection
        DeckBuildingScreen.Instance.CollectionBrowserScript.Asset = tab.Asset;
        DeckBuildingScreen.Instance.CollectionBrowserScript.IncludeAllCharacters = tab.showAllCharacters;
    }

    public void SetClassOnClassTab(FactionAsset asset)
    {
        ClassTab.Asset = asset;
        ClassTab.GetComponentInChildren<Text>().text = asset.name;
    }
}

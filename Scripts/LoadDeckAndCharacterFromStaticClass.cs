using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDeckAndCharacterFromStaticClass : MonoBehaviour {

    void Awake()
    {
        Player p = GetComponent<Player>();
        if (p.ID == 2)
        {
            if (BattleStartInfo.SelectedDeck != null)
            {
                if (BattleStartInfo.SelectedDeck.heroAsset != null)
                    p.heroAsset = BattleStartInfo.SelectedDeck.heroAsset;
                if (BattleStartInfo.SelectedDeck.Cards != null)
                    p.deck.cards = new List<CardAsset>(BattleStartInfo.SelectedDeck.Cards);
            }
        }
        else if (p.ID == 1)
        {
            if (BattleStartInfo.EmenyDeck != null)
            {
                if (BattleStartInfo.EmenyDeck.heroAsset != null)
                {
                    p.heroAsset = BattleStartInfo.EmenyDeck.heroAsset;
                }
                if (BattleStartInfo.EmenyDeck.Cards != null)
                {
                    p.deck.cards = new List<CardAsset>(BattleStartInfo.EmenyDeck.Cards);
                }
            }
        }
       

    }
}

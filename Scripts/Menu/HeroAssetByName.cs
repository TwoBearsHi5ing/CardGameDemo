using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAssetByName : MonoBehaviour
{
    public static HeroAssetByName Instance;
    private HeroAsset[] allCharacterAssets; 
    private Dictionary<string, HeroAsset> AllCharactersDictionary = new Dictionary<string, HeroAsset>();

    void Awake()
    {
        Instance = this;
        allCharacterAssets = Resources.LoadAll<HeroAsset>("");

        foreach (HeroAsset ca in allCharacterAssets)
            if(!AllCharactersDictionary.ContainsKey(ca.name))
                AllCharactersDictionary.Add(ca.name, ca);
    }

    public HeroAsset GetCharacterByName(string name)
    {
        if (AllCharactersDictionary.ContainsKey(name))
            return AllCharactersDictionary[name];
        else
            return null;
    }
}

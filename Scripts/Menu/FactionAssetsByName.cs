using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionAssetsByName : MonoBehaviour {

    public static FactionAssetsByName Instance;
    private FactionAsset[] allCharacterAssets; 
    private Dictionary<string, FactionAsset> AllCharactersDictionary = new Dictionary<string, FactionAsset>();

    void Awake()
    {
        Instance = this;
        allCharacterAssets = Resources.LoadAll<FactionAsset>("");

        foreach (FactionAsset ca in allCharacterAssets)
            if(!AllCharactersDictionary.ContainsKey(ca.name))
                AllCharactersDictionary.Add(ca.name, ca);
    }

    public FactionAsset GetCharacterByName(string name)
    {
        if (AllCharactersDictionary.ContainsKey(name))
            return AllCharactersDictionary[name];
        else
            return null;
    }
}

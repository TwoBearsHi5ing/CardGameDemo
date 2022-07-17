using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

public enum TargetingOptions
{
    NoTarget,
    AllUnits, 
    EnemyUnits,
    YourUnits 
   // AllCharacters, 
    //EnemyCharacters,
    //YourCharacters
}

public enum MovementOptions
{
    Vertical,
    Horizontal,
    Ver_Hor,
    Diagonally,
    All_Ways
}

public enum AttackType
{
   Melee,
   Range
}
public enum TypesOfCards
{
    Unit, Spell
}
public enum RarityOptions
{
    Basic, Common, Rare, Epic, Legendary
}

public class CardAsset : ScriptableObject, IComparable<CardAsset>
{
    // this object will hold the info about the most general card
    [Header("General info")]
    public FactionAsset factionAsset;
    [TextArea(2, 3)]
    public string Description;  
    [TextArea(2, 3)]
    public string Tags; 
    [PreviewField]
    public Sprite CardImage;
    public int AP_Cost;
    [HideInInspector]
    public RarityOptions Rarity = RarityOptions.Basic;
    [HideInInspector]
    public int OverrideLimitOfThisCardInDeck = -1;

    [EnumToggleButtons]
    public TypesOfCards TypeOfCard;

    [Header("Unit Info")]

    [Range(0, 30)]
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public int MaxHealth;  

    [Range(0, 30)]
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public int Attack;

    [Range(0, 5)]
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public int MovePoints;

    [Range(0, 3)]
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public int MoveRange;

    [Range(0, 3)]
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public int AttackRange;

    bool Taunt;
    bool Charge;

    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public string UnitScriptName;

    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public int specialUnitAmount;

 
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public MovementOptions moveOption;


    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public MovementOptions attackOption;

    [EnumToggleButtons]
    [ShowIf("TypeOfCard", TypesOfCards.Unit)]
    public AttackType attackType;

    [Header("SpellInfo")]

    [ShowIf("TypeOfCard", TypesOfCards.Spell)]
    public string SpellScriptName;

    [ShowIf("TypeOfCard", TypesOfCards.Spell)]
    public int specialSpellAmount;

    [ShowIf("TypeOfCard", TypesOfCards.Spell)]
    public int numberOfturns;

    [EnumToggleButtons]
    [ShowIf("TypeOfCard", TypesOfCards.Spell)]
    public TargetingOptions Targets;

    public int CompareTo(CardAsset other)
    {
        if (other.AP_Cost < this.AP_Cost)
        {
            return 1;
        }
        else if (other.AP_Cost > this.AP_Cost)
        {
            return -1;
        }
        else
        {
            // if mana costs are equal sort in alphabetical order
            return name.CompareTo(other.name);
        }
    }

    public static bool operator > (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == 1;
    }

    // Define the is less than operator.
    public static bool operator < (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == -1;
    }

    // Define the is greater than or equal to operator.
    public static bool operator >= (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    // Define the is less than or equal to operator.
    public static bool operator <= (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }
}

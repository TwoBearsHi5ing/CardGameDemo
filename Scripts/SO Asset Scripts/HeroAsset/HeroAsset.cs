using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public enum CharClass{ Hunter, Wizard, Warrior}


public class HeroAsset : ScriptableObject 
{
    public FactionAsset faction;
    
    public CharClass Class;
    [HideInInspector]
    public string HeroName;
    [Range(0, 30)]
    public int MaxHealth;

    [HideInInspector]
	public string HeroPowerName;

    [PreviewField]
    public Sprite HeroImage;

    [HideInInspector]
    public Sprite HeroPowerIconImage;
    [HideInInspector]
    public Sprite AvatarBGImage;
    [HideInInspector]
    public Sprite HeroPowerBGImage;
    [HideInInspector]
    public Color32 AvatarBGTint;
    [HideInInspector]
    public Color32 HeroPowerBGTint;


    [TextArea(2, 3)]
    public string Description;  
    public MovementOptions moveOption;
    public MovementOptions attackOption;
    [EnumToggleButtons]
    public AttackType attackType;

    [Range(0, 5)]
    public int Attack;
    [Range(0, 3)]
    public int MoveRange;
    [Range(0, 3)]
    public int AttackRange;
    [Range(0, 5)]
    public int AP;
    [HideInInspector]
    public TargetingOptions Targets;




}

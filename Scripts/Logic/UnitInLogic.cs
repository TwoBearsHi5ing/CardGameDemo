using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UnitInLogic: IUnit 
{
    public Player owner;
    public CardAsset ca;
    public UnitEffect effect;
    public HeroAsset ha;
    public int UniqueUnitID;
    public int Position;
    public bool SlotFree;
   
    public bool Frozen = false;

   
    public int ID
    {
        get{ return UniqueUnitID; }
    }

   
    public int _UnitPosition
    {
        get
        {
            return Position;        
        }

        set
        {
            if (value <= 0)
            {
                Position = 0;
            }
            else if (value > 23)
            {
                Position = 23;
            }
            else 
            {
                Position = value;
            }
        }
    }   
        
    // the basic health that we have in CardAsset
    private int baseHealth;
    // health with all the current buffs taken into account
    public int MaxHealth
    {
        get{ return baseHealth;}
    }

    // current health of this creature
    private int health;
    public int Health
    {
        get{ return health; }

        set
        {
            if (value > MaxHealth)
                health = MaxHealth;
            else if (value <= 0)
                Die();
            else
                health = value;

        }
    }

    // returns true if we can attack with this creature now
    public bool CanAttack
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
            return (ownersTurn && (MovePoints > 0) && !Frozen);
        }
    }

    // property for Attack
    private int baseAttack;
    public int Attack
    {
        get
        {
            return baseAttack;
        }
        set
        {
            baseAttack = value;
        }
    }


    private int actionPerTurn;
    public int MovePoints { get; set; }
    
    


    private AttackType _attackType;
    public AttackType attackType
    {
        get;
        set;
    }
  
    

    public UnitInLogic(Player owner, CardAsset ca, int Position, bool freeSlot)
    {
        this.ca = ca;
        baseHealth = ca.MaxHealth;
        Health = ca.MaxHealth;
        baseAttack = ca.Attack;
        actionPerTurn = ca.MovePoints;
        _UnitPosition = Position;
        SlotFree = freeSlot;
        attackType = ca.attackType;
     
        this.owner = owner;
        UniqueUnitID = IDFactory.GetUniqueID();
        if (ca.UnitScriptName!= null && ca.UnitScriptName!= "")
        {
            //Debug.Log("creature  effect : " + ca.UnitScriptName);
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.UnitScriptName), new System.Object[]{owner, this, ca.specialUnitAmount}) as UnitEffect;
            effect.RegisterEventEffect();
        }
        UnitsCreatedThisGame.Add(UniqueUnitID, this);
    }

    public UnitInLogic(Player owner, HeroAsset ha, int Position, bool freeSlot)
    {
        this.ha = ha;
        baseHealth = ha.MaxHealth;
        Health = ha.MaxHealth;
        baseAttack = ha.Attack;
        actionPerTurn = ha.AP;
        _UnitPosition = Position;
        SlotFree = freeSlot;
        attackType = ha.attackType;

        this.owner = owner;
        UniqueUnitID = IDFactory.GetUniqueID();
      
        UnitsCreatedThisGame.Add(UniqueUnitID, this);
    }

    public UnitInLogic(Player owner, CardAsset c, HeroAsset h, bool freeSlot)
    {
        this.owner = owner;
        ca = c;
        ha = h;
        SlotFree = freeSlot;
      
        
    }
   
    public static UnitInLogic FindUnitLogicByID(int ID)
    {
        foreach (UnitInLogic cl in Table.instance.UnitsOnTable)
        { 
            if (cl.ID == ID)
            {
                return cl;
            }
        }
        return null;
    }
    public void OnTurnStart()
    {
        MovePoints = actionPerTurn;
        UpdateValues(Attack, MovePoints, health, UniqueUnitID);  
    }

    public void UpdateValues(int atk, int mp, int hp, int id)
    {
        if (owner != null)
        {
            new UpdateCreatureValuesCommand(atk, mp, hp, id).AddToQueue();
        }
         
    }
    public void Die()
    {

        
        int TablePos = Table.instance.FindUnitOnTable(this);
       // Debug.LogWarning("index w die() : " + UniqueUnitID);
        if (UniqueUnitID == 4 || UniqueUnitID == 6)
        {
            owner.Health = 0;
           
        }
        else
        {
         
            if (effect != null)
            {
                effect.WhenUnitDies();
                effect.UnRegisterEventEffect();
                effect = null;
            }
            Table.instance.RemoveUnitAt(TablePos);

            new CreatureDieCommand(UniqueUnitID, owner, TablePos).AddToQueue();
        }
    }

    public void GoFace()
    {
        MovePoints--;
        int actionleftAfterAttack = MovePoints;
        int targetHealthAfter = owner.otherPlayer.Health - Attack;
        new CreatureAttackCommand(owner.otherPlayer.PlayerID, UniqueUnitID, 0, Attack, Health, targetHealthAfter, actionleftAfterAttack).AddToQueue();
        owner.otherPlayer.Health -= Attack;
        UpdateValues(Attack, MovePoints, health, UniqueUnitID);
        
    }

    public void AttackUnit (UnitInLogic target)
    {
        MovePoints--;
        int actionleftAfterAttack = MovePoints;
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;

        if (attackType == AttackType.Melee)
        {
          
            new CreatureAttackCommand(target.UniqueUnitID, UniqueUnitID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter , actionleftAfterAttack).AddToQueue();

            target.Health -= Attack;
            Health -= target.Attack;
        }
        else if (attackType == AttackType.Range)
        {
           
            new CreatureAttackCommand(target.UniqueUnitID, UniqueUnitID, 0, Attack, health, targetHealthAfter , actionleftAfterAttack).AddToQueue();
            target.Health -= Attack;
        }
        UpdateValues(Attack, MovePoints, health, UniqueUnitID);
        target.UpdateValues(target.Attack, target.MovePoints, target.health, target.UniqueUnitID);
    }

    public void AttackUnitWithID(int uniqueCreatureID)
    {
        UnitInLogic target = UnitInLogic.UnitsCreatedThisGame[uniqueCreatureID];
        AttackUnit(target);
    }

    public void MoveUnit(int uniqueCreatureID, int new_position, Vector3 animation_position)
    {
        UnitInLogic newCreaturePosition = UnitsCreatedThisGame[uniqueCreatureID];
      //  Debug.Log("id in creature logic : " + newCreaturePosition.ID);
        newCreaturePosition.Position = new_position;
        int i = Table.instance.FindUnitOnTable(newCreaturePosition);

        if (i >= 0)
        {
            MovePoints--;
            int actionleftAfterMoveing = MovePoints;
            Table.instance.PlaceUnitAt(new_position, newCreaturePosition);
            Table.instance.RemoveUnitAt(i);
       
            UnitsCreatedThisGame.Remove(uniqueCreatureID);
            UnitsCreatedThisGame.Add(uniqueCreatureID, newCreaturePosition);
            new CreatureMoveCommand(uniqueCreatureID, animation_position, actionleftAfterMoveing).AddToQueue();
        }

        UpdateValues(Attack, MovePoints, health, UniqueUnitID);




    }

    public static Dictionary<int, UnitInLogic> UnitsCreatedThisGame = new Dictionary<int, UnitInLogic>();

}

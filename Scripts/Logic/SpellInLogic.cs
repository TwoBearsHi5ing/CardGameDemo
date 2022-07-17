using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpellInLogic : I_ID
{
    public Player owner;
    public CardAsset ca;
    public TableSpellEffect effect;
    public int UniqueSpellID;
    public int Position;
    public bool SlotFree;

    public bool Frozen = false;

    public static Dictionary<int, SpellInLogic> SpellsActive = new Dictionary<int, SpellInLogic>();

  
    public int ID
    {
        get { return UniqueSpellID; }
    }


    public int _SpellPosition
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
            else if (value > 3)
            {
                Position = 3;
            }
            else
            {
                Position = value;
            }
        }
    }

    public int _turnAmount;
    public int TurnAmount
    {
        get 
        {
            return _turnAmount;
        }
        set 
        {
            if (value > 0)
            {
                _turnAmount = value;
            }
            if (value <= 0)
            {
                Expire();
            }
        }
    }

 

    public SpellInLogic(Player owner, CardAsset ca, int Position, bool freeSlot)
    {
        this.ca = ca;
        _SpellPosition = Position;
        SlotFree = freeSlot;
        TurnAmount = ca.numberOfturns;  
        
        this.owner = owner;
        UniqueSpellID = IDFactory.GetUniqueID();
        //Debug.LogWarning("spell on table id : " + UniqueSpellID);

        if (ca.SpellScriptName != null && ca.SpellScriptName != "")
        {
           // Debug.Log("table spell effect : " + ca.SpellScriptName );
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName), new System.Object[] { owner, this, ca.specialSpellAmount, TurnAmount }) as TableSpellEffect;
            effect.RegisterEventEffect();
        }
        SpellsActive.Add(UniqueSpellID, this);
    }

    public SpellInLogic(Player owner, CardAsset c, bool freeSlot)
    {
        this.owner = owner;
        ca = c;
       
        SlotFree = freeSlot;


    }
    public void UpdateValues(int TurnsLeft, int id)
    {
        if (owner != null)
        {
            new UpdateSpellValuesCommand(TurnsLeft, id).AddToQueue();
        }

    }
    public void OnTurnStart()
    {
        TurnAmount--;
        UpdateValues(TurnAmount, UniqueSpellID);
    }

    public void Expire()
    {
        int index = Table.instance.FindSpellOnTable(this);

        Table.instance.RemoveSpellAt(index);

        if (effect != null)
        {
            effect.WhenSpellExpires();
            effect.UnRegisterEventEffect();
            effect = null;
        }

        new SpellExpireCommand(UniqueSpellID, owner, index).AddToQueue();
    }








}

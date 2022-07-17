using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public List<UnitInLogic> UnitsOnTable = new List<UnitInLogic>();
    public List<SpellInLogic> SpellsOnTable = new List<SpellInLogic>();

    public static Table instance;

    UnitInLogic cl = new UnitInLogic(null, null, null, true);
    SpellInLogic sl = new SpellInLogic(null, null, true);

    public void PlaceUnitAt(int index, UnitInLogic creature)
    {
        instance.UnitsOnTable[index] = creature;
    }
    public void RemoveUnitAt(int index)
    {
        instance.UnitsOnTable[index] = cl;
    }

    public void PlaceSpellAt(int index, SpellInLogic spell)
    {
        instance.SpellsOnTable[index] = spell;
    }
    public void RemoveSpellAt(int index)
    {
        instance.SpellsOnTable[index] = sl;
    }

    public bool ChechkIfUnitSlotIsFree(int position)
    {
        if (position >= 0 && position < 24)
        {
            if (instance.UnitsOnTable[position].SlotFree == true)
            {
                return true;
            }
        }
        return false;
    }

    public bool ChechkIfSpellSlotIsFree(int position)
    {
        if (position >= 0 && position < 4)
        {
            if (instance.SpellsOnTable[position].SlotFree == true)
            {
                return true;
            }
        }

        return false;

    }

    public int FindUnitOnTable(UnitInLogic cl)
    {
        int i = 0;

        foreach (UnitInLogic c in instance.UnitsOnTable)
        {
            if (c.ID == cl.ID)
            {
                //Debug.Log("i poprawne : " + i);
                return i;
            }
            i++;
        }
       // Debug.Log("nie znaleziono");
        return -1;
    }
    public UnitInLogic FindUnitByID(int id)
    {
        foreach (UnitInLogic c in instance.UnitsOnTable)
        {
            if (c.ID == id)
            {
                return c;
            }
        }
        return null;
    }
    public int FindUnitByPosition(int position)
    {
        foreach (UnitInLogic c in instance.UnitsOnTable)
        {
            if (c.Position == position)
            {
                return c.UniqueUnitID;
            }
        }
        return -1;
    }

    public int FindSpellOnTable(SpellInLogic sl)
    {
        int i = 0;

        foreach (SpellInLogic s in instance.SpellsOnTable)
        {
            if (s.ID == sl.ID)
            {
                //Debug.Log("i poprawne : " + i);
                return i;
            }
            i++;
        }
        //Debug.Log("zwracanie -1");
        return -1;
    }
    private void Awake()
    {
       
        instance = this;

        //  Debug.Log("Creatures On table Visual: " + CreaturesOnTable.Count);

        if (instance.UnitsOnTable.Count == 0)
        {
            for (int i = 0; i < 24; i++)
            {
                instance.UnitsOnTable.Add(cl);
            }

        }
        if (instance.SpellsOnTable.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                instance.SpellsOnTable.Add(sl);
            }

        }

    }
}

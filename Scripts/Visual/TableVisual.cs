using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TableVisual : MonoBehaviour 
{  
    public AreaPosition owner1;
    public TabeSlots tableSlots;
    public SpellSlots spellSlots;

    // list of all the creature cards on the table as GameObjects
    private  List<GameObject> CreaturesOnTable = new List<GameObject>();
    private List<GameObject> SpellsOnTable = new List<GameObject>();

    // are we hovering over this table`s collider with a mouse
    private bool cursorOverThisTable = false;

    // A 3D collider attached to this game object
    private BoxCollider col;

    public GameObject DeckPosition;
    public GameObject HeroPreviewPosition;

    // returns true if we are hovering over any player`s table collider
    public static bool CursorOverSomeTable
    {
        get
        {
            TableVisual bothTables = GameObject.FindObjectOfType<TableVisual>();
            return (bothTables.CursorOverThisTable);
        }
    }

    // returns true only if we are hovering over this table`s collider
    public bool CursorOverThisTable
    {
        get{ return cursorOverThisTable; }
    }

    
    void Awake()
    {
        col = GetComponent<BoxCollider>();


        if (CreaturesOnTable.Count == 0)
        {
            for (int i = 0; i < 24; i++)
            {
                CreaturesOnTable.Add(null);
            }
        }
        if (SpellsOnTable.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                SpellsOnTable.Add(null);
            }
        }


    }

    void Update()
    {
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        // create an array of RaycastHits
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        cursorOverThisTable = passedThroughTableCollider;

        
    }
    public void PlaySpellOnTable(int UniqueID, int tablePos, CardAsset ca, int _owner)
    {
       

        if (_owner == 2)
        {
            owner1 = AreaPosition.Low;
        }

        if (_owner == 1)
        {
            owner1 = AreaPosition.Top;
        }


        GameObject SpellOnTable = GameObject.Instantiate(GlobalSettings.Instance.SpellOnTablePrefab, spellSlots.All_Spell_Slots[tablePos].transform.position, Quaternion.identity) as GameObject;
        SpellOnTable.transform.localScale = new Vector2(1.15f, 1.15f);

        // apply the look from CardAsset
        OneSpellManager manager = SpellOnTable.GetComponent<OneSpellManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();

        foreach (Transform t in SpellOnTable.GetComponentsInChildren<Transform>())
            t.tag = owner1.ToString() + "Spell";


        // parent a new creature gameObject to table slots
        SpellOnTable.transform.SetParent(tableSlots.transform);

        // add a new creature to the list
        SpellsOnTable[tablePos] = SpellOnTable;

        // let this creature know about its position
        WhereIsTheCardOrCreature w = SpellOnTable.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = tablePos;
        w.SetSpellSortingOrder();


        // add our unique ID to this creature
        IDHolder id = SpellOnTable.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

       

        // end command execution
        Command.CommandExecutionComplete();
         
    }

    // method to create a new creature and add it to the table
    public void AddCreatureAtIndex(CardAsset ca, int UniqueID ,int index, int _owner)
    {

        if (_owner == 2)
        {
            owner1 = AreaPosition.Low;
        }

        if (_owner == 1)
        {
            owner1 = AreaPosition.Top;
        }



        GameObject creature = GameObject.Instantiate(GlobalSettings.Instance.CreaturePrefab, tableSlots.All_Creature_slots[index].transform.position, Quaternion.identity) as GameObject;
        creature.transform.localScale = new Vector2(1.15f, 1.15f);
        OneUnitManager manager = creature.GetComponent<OneUnitManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();

        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            t.tag = owner1.ToString()+"Creature";
        
        creature.transform.SetParent(tableSlots.transform);
        CreaturesOnTable[index] = creature;

        WhereIsTheCardOrCreature w = creature.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
    
       

        IDHolder id = creature.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        Command.CommandExecutionComplete();
    }

    public void AddHeroAtIndex(HeroAsset ha, int UniqueID, int index, int _owner)
    {
        int HeroPosition = 0;

        if (_owner == 2)
        {
            owner1 = AreaPosition.Low;
            HeroPosition = 0;
            DeckPosition.transform.position = new Vector3(17, -5, 0);
            HeroPreviewPosition.transform.position = new Vector3(-1, 0.6f, 0);
        }

        if (_owner == 1)
        {
            owner1 = AreaPosition.Top;
            HeroPosition = 23;
            DeckPosition.transform.position = new Vector3(17, 6.2f, 0);
            HeroPreviewPosition.transform.position = new Vector3(1, 0.6f, 0);
        }



        GameObject hero = GameObject.Instantiate(GlobalSettings.Instance.HeroPrefab, DeckPosition.transform.position, Quaternion.identity) as GameObject;
        hero.transform.localScale = new Vector2(1.15f, 1.15f);
        OneHeroManager manager = hero.GetComponent<OneHeroManager>();
        manager.heroAsset = ha;
        manager.ReadCreatureFromAsset();

        foreach (Transform t in hero.GetComponentsInChildren<Transform>())
            t.tag = owner1.ToString() + "Creature";

        CreaturesOnTable[index] = hero;

        WhereIsTheCardOrCreature w = hero.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;

     
        w.VisualState = VisualStates.Table;

        IDHolder id = hero.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        hero.transform.rotation = Quaternion.Euler(0,180,0);

        Sequence s = DOTween.Sequence();
       
        
            
        s.Append(hero.transform.DOMove(HeroPreviewPosition.transform.position, GlobalSettings.Instance.CardTransitionTime));

        s.Append(hero.transform.DOLocalRotateQuaternion(Quaternion.identity, GlobalSettings.Instance.CardTransitionTime));

        s.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
       
        s.Append(hero.transform.DOLocalMove(tableSlots.All_Creature_slots[HeroPosition].transform.position, GlobalSettings.Instance.CardTransitionTime));

        s.OnComplete(() => 
        {
            hero.transform.SetParent(tableSlots.transform);
            HoverPreview.PreviewsAllowed = true;
        });

        

        Command.CommandExecutionComplete();
    }


    


    public int TablePositionForNewUnit(float MouseX, float MouseY)
    {
        for (int i = 0; i < 24; i++)
        {
            int slot;
            float maxX = tableSlots.All_Creature_slots[i].transform.position.x + 1.0f;
            float minX = tableSlots.All_Creature_slots[i].transform.position.x - 1.0f;
            float maxY = tableSlots.All_Creature_slots[i].transform.position.y + 1.5f;
            float minY = tableSlots.All_Creature_slots[i].transform.position.y - 1.5f;

            if (MouseX > -0.5 && MouseX < 0.5)
            {
                //Debug.LogWarning("Nie znaleziono miejsca. Zwracanie -1");
                return -1;
            }

            else if (((MouseX < tableSlots.All_Creature_slots[i].transform.position.x && MouseX > minX) || (MouseX > tableSlots.All_Creature_slots[i].transform.position.x && MouseX < maxX)) &&
                ((MouseY < tableSlots.All_Creature_slots[i].transform.position.y && MouseY > minY) || (MouseY > tableSlots.All_Creature_slots[i].transform.position.y && MouseY < maxY)))
            {
             
                slot = i;
               // Debug.Log("Pozycja: " + slot );
                return slot;
            }

        }
        //Debug.LogWarning("Nie znaleziono miejsca. Zwracanie -1");
        return -1;
        

    }

    public Vector3 TablePosForCreature(float MouseX, float MouseY)
    {
       
        for (int i = 0; i < 24; i++)
        {
            float target_x;
            float target_y;
            float maxX = tableSlots.All_Creature_slots[i].transform.position.x + 1.0f;
            float minX = tableSlots.All_Creature_slots[i].transform.position.x - 1.0f;
            float maxY = tableSlots.All_Creature_slots[i].transform.position.y + 1.5f;
            float minY = tableSlots.All_Creature_slots[i].transform.position.y - 1.5f;

            if (MouseX > -0.5 && MouseX < 0.5)
            {
                //Debug.LogWarning("Nie znaleziono miejsca. Zwracanie -1");
                return Vector3.zero;
            }

            else if (((MouseX < tableSlots.All_Creature_slots[i].transform.position.x && MouseX > minX) || (MouseX > tableSlots.All_Creature_slots[i].transform.position.x && MouseX < maxX)) &&
                ((MouseY < tableSlots.All_Creature_slots[i].transform.position.y && MouseY > minY) || (MouseY > tableSlots.All_Creature_slots[i].transform.position.y && MouseY < maxY)))
            {

                target_x = tableSlots.All_Creature_slots[i].transform.position.x;
                target_y = tableSlots.All_Creature_slots[i].transform.position.y;
                return (new Vector3 (target_x, target_y , 0));
            }

        }
        //Debug.LogWarning("Nie znaleziono miejsca. Zwracanie -1");
        return Vector3.zero;


    }

    public int NewTablePosForNewSpell(float MouseX, float MouseY)
    {
        for (int i = 0; i < 4; i++)
        {
            int slot;
            float maxX = spellSlots.All_Spell_Slots[i].transform.position.x + 1.0f;
            float minX = spellSlots.All_Spell_Slots[i].transform.position.x - 1.0f;
            float maxY = spellSlots.All_Spell_Slots[i].transform.position.y + 1.5f;
            float minY = spellSlots.All_Spell_Slots[i].transform.position.y - 1.5f;

            if (((MouseX < spellSlots.All_Spell_Slots[i].transform.position.x && MouseX > minX) || (MouseX > spellSlots.All_Spell_Slots[i].transform.position.x && MouseX < maxX)) &&
                ((MouseY < spellSlots.All_Spell_Slots[i].transform.position.y && MouseY > minY) || (MouseY > spellSlots.All_Spell_Slots[i].transform.position.y && MouseY < maxY)))
            {
                slot = i;
                return slot;
            }

            else
            {
                slot = -1;
            }

           
        }

        return -1;

    }

    public void RemoveCreatureWithID(int IDToRemove, int index)
    {
        
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        CreaturesOnTable[index] = null;
        Destroy(creatureToRemove);

        Command.CommandExecutionComplete();
    }
    public void RemoveSpellWithID(int IDToRemove, int index)
    {
      
        GameObject SpellToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        SpellsOnTable[index] = null;
        Destroy(SpellToRemove);

        Command.CommandExecutionComplete();
    }


}

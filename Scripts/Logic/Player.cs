using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IUnit
{
   
    public int PlayerID;
    public HeroAsset heroAsset;
    public PlayerArea PArea;
    
    public Deck deck;
    public Hand hand;
    
    public static Player[] Players;

    private int bonusAPThisTurn = 0;

    public int ID
    {
        get{ return PlayerID; }
    }

    public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }

    private int apThisTurn;
    public int APThisTurn
    {
        get { return apThisTurn; }
        set
        {
            if (value < 0)
            {
                apThisTurn = 0;       
            }

            else if (value > PArea.AP_Bar.MAX_AP)
            {
                apThisTurn = PArea.AP_Bar.MAX_AP;         
            }

            else
            {
                apThisTurn = value;
            }

            new UpdateManaCrystalsCommand(this, apThisTurn, apLeft).AddToQueue();
        }
    }

    private int apLeft;
    public int ApLeft
    {
        get
        { return apLeft;}
        set
        {
            if (value < 0)
            {
                apLeft = 0;
            }
            else if (value > PArea.AP_Bar.MAX_AP)
            {
                apLeft = PArea.AP_Bar.MAX_AP;
            }

            else
            {
                apLeft = value;
            }             
            
            new UpdateManaCrystalsCommand(this, APThisTurn, apLeft).AddToQueue();
            if (TurnManager.Instance.whoseTurn == this)
                HighlightPlayableCards();
        }
    }

    private int health;
    public int Health
    {
        get { return health;}
        set
        {
            if (value > heroAsset.MaxHealth)
                health = heroAsset.MaxHealth;
            else
                health = value;
            if (value <= 0)
                Die(); 
        }
    }

    private int numberOfTurns;
    public int NumberOfTurns
    {
        get { return numberOfTurns; }

        set
        {
            numberOfTurns = value;
            PArea.testNumberOfTurns.text = numberOfTurns.ToString();
        }
    }

    public int MovePoints { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int Attack { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public delegate void VoidWithNoArguments();
   
    public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;


    public Player FindPlayerWithID(int ID)
    {
        if (ID == PlayerID)
        {
            return this;
        }
        else 
            return null;
    }
    void Start()
    {
        Health = heroAsset.MaxHealth;
     
    }
   
    void Awake()
    { 
        Players = GameObject.FindObjectsOfType<Player>();
        PlayerID = IDFactory.GetUniqueID();

    }

    public virtual void OnTurnStart()
    {
        if (StartTurnEvent != null)
        {
            StartTurnEvent.Invoke();
        }

        NumberOfTurns++;
        APThisTurn++;
        ApLeft = APThisTurn;

        foreach (UnitInLogic cl in Table.instance.UnitsOnTable)
        {
            if (cl != null)
            {
                cl.OnTurnStart();
            }
        }
      
        foreach (SpellInLogic sl in Table.instance.SpellsOnTable.ToArray())
        {

            if (sl.owner != null && sl.owner == TurnManager.Instance.whoseTurn )
            {
                sl.OnTurnStart();
            }
        }
      
    }

    public void OnTurnEnd()
    {

        if (EndTurnEvent != null)
        {
            EndTurnEvent.Invoke();
        }
            
        APThisTurn -= bonusAPThisTurn;
        bonusAPThisTurn = 0;
        GetComponent<TurnOwner>().StopAllCoroutines();
    }
    public void HighlightPlayableCards(bool removeAllHighlights = false)
    {
        foreach (CardInLogic cl in hand.CardsInHand)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
            if (g != null)
                g.GetComponent<OneCardManager>().CanBePlayedNow = (cl.CurrentAPCost <= ApLeft) && !removeAllHighlights;
        }

        foreach (UnitInLogic crl in Table.instance.UnitsOnTable)
        {
            if (crl != null)
            {
                GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueUnitID);
                if (g != null)
                {
                    if (g.GetComponent<OneUnitManager>() != null)
                    {
                        g.GetComponent<OneUnitManager>().CanAttackNow = (crl.MovePoints > 0) && !removeAllHighlights;
                    }
                    else if (g.GetComponent<OneHeroManager>() != null)
                    { 
                        g.GetComponent<OneHeroManager>().CanAttackNow = (crl.MovePoints > 0) && !removeAllHighlights;
                    }
                   
                }
                    
            }
           
        }
    }

    public void GetApBonus(int amount)
    {
        bonusAPThisTurn += amount;
        APThisTurn += amount;
        ApLeft += amount;

        DamageEffect.CreateDamageEffect(new Vector3(PArea.AP_Bar.transform.position.x - 1, PArea.AP_Bar.transform.position.y - 0.2f, PArea.AP_Bar.transform.position.z) , amount, Visual_Effect.ManaBonus);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D))
           // DrawACard();
    }

    public void DrawACard(bool fast = false)
    {
        if (deck.cards.Count > 0)
        {
            if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
            {
                CardInLogic newCard = new CardInLogic(deck.cards[0]);
                newCard.owner = this;
                hand.CardsInHand.Insert(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                deck.cards.RemoveAt(0);
                new DrawACardCommand(hand.CardsInHand[0], this, fast, fromDeck: true).AddToQueue(); 
            }
        }
        else
        {
            Health--;
        }
       
    }

    public void GetACardNotFromDeck(CardAsset cardAsset)
    {
        if (hand.CardsInHand.Count < PArea.handVisual.slots.Children.Length)
        {
            CardInLogic newCard = new CardInLogic(cardAsset);
            newCard.owner = this;
            hand.CardsInHand.Insert(0, newCard);
            new DrawACardCommand(hand.CardsInHand[0], this, fast: true, fromDeck: false).AddToQueue(); 
        }
    }

    
    public void PlaySpellFromHand(int SpellCardUniqueID, int TargetUniqueID)
    {
        PlaySpellFromHand(CardInLogic.CardsCreatedThisGame[SpellCardUniqueID], UnitInLogic.UnitsCreatedThisGame[TargetUniqueID]);    
    }
    public void PlaySpellFromHand(CardInLogic playedCard, IUnit target)
    {
        hand.CardsInHand.Remove(playedCard);
        ApLeft -= playedCard.CurrentAPCost;
        if (playedCard.effect != null)
            playedCard.effect.ActivateEffect(playedCard._cardAsset.specialSpellAmount, target);

        else
        {
            Debug.LogWarning("No effect found on card " + playedCard._cardAsset.name);
        }

    

      
            new PlayASpellCardCommand(this, playedCard).AddToQueue();
      
    }


    public void PlayUnitFromHand(int UniqueID, int tablePos)
    {
        PlayUnitFromHand(CardInLogic.CardsCreatedThisGame[UniqueID], tablePos);
    }

    public void PlayUnitFromHand(CardInLogic playedCard, int tablePos)
    {
        // Debug.Log(ManaLeft);
        // Debug.Log(playedCard.CurrentManaCost);
        ApLeft -= playedCard.CurrentAPCost;
        // Debug.Log("Mana Left after played a creature: " + ManaLeft);

        UnitInLogic newCreature = new UnitInLogic(this, playedCard._cardAsset,tablePos, false);
        Table.instance.UnitsOnTable[tablePos] = newCreature;
        // 
        new PlayACreatureCommand(playedCard, this, tablePos, newCreature.UniqueUnitID).AddToQueue();

        if (newCreature.effect != null)
            newCreature.effect.WhenUnitIsPlayed();

        hand.CardsInHand.Remove(playedCard);
        HighlightPlayableCards();
    }

    public void PlaySpellOnTable(int UniqueID, int tablePos)
    {
        if (tablePos >= 0 && tablePos < 4)
        {
            PlaySpellOnTable(CardInLogic.CardsCreatedThisGame[UniqueID], tablePos);
        }
        
    }
    public void PlaySpellOnTable(CardInLogic playedCard, int tablePos)
    {
        hand.CardsInHand.Remove(playedCard);
        ApLeft -= playedCard.CurrentAPCost;
        SpellInLogic newSpell = new SpellInLogic(this, playedCard._cardAsset, tablePos, false);
        Table.instance.SpellsOnTable[tablePos] = newSpell;


        new PlayASpellOnTableCommand(this, playedCard, newSpell,tablePos,newSpell.ID).AddToQueue();

        if (newSpell.effect != null)
        {
            newSpell.effect.WhenASpellIsPlayed();
        }
       
        HighlightPlayableCards();
    }


    public void PlayHero(PlayerArea pa)
    {
        int TablePos;
        if (pa.owner == AreaPosition.Low)
        {
            TablePos = 0;
        }
        else
        {
            TablePos = 23;
        }

        CardInLogic newCard = new CardInLogic(heroAsset);
        newCard.owner = this;

  

        UnitInLogic hero = new UnitInLogic(this, heroAsset, TablePos, false);


        Table.instance.UnitsOnTable[TablePos] = hero;


        new PlayHeroCommand(newCard, this, TablePos, hero.UniqueUnitID).AddToQueue();


    }

   

    public void Die()
    {
        int id = -1;

        if (this.tag == "LowPlayer")
        {
            id = 6;
        }
        else if (this.tag == "TopPlayer")
        {
            id = 4;
        }
        else
        {
            Debug.LogError("Blad! : zle id");
        }
        if (id > 0)
        {
            // game over
            // block both players from taking new moves 
            TurnManager.Instance.StopTimer();
            PArea.ControlsON = false;
            otherPlayer.PArea.ControlsON = false;
            GameObject hero = IDHolder.GetGameObjectWithID(id);
            new GameOverCommand(hero).AddToQueue();
        }
        
    }
    

    // nie uzywane !!!!!!!!
    public void LoadHeroInfoFromAsset()
    {
        Health = heroAsset.MaxHealth;
        PArea.oneHeroManager.heroAsset = heroAsset;
       // Debug.Log("null na 100 % "+PArea.oneHeroManager);
        PArea.oneHeroManager.ReadCreatureFromAsset();

      /*  if (heroAsset.HeroPowerName != null && heroAsset.HeroPowerName != "")
        {
            HeroPowerEffect = System.Activator.CreateInstance(System.Type.GetType(heroAsset.HeroPowerName)) as SpellEffect;
        }
        else
        {
            Debug.LogWarning("Check hero powr name for character " + heroAsset.HeroName);
        }
      */
    }

    public void TransmitInfoAboutPlayerToVisual()
    {
        if (GetComponent<TurnOwner>() is TurnOwnerAI)
        {
            PArea.AllowedToControlThisPlayer = false;
        }
        else
        {
            PArea.AllowedToControlThisPlayer = true;
        }
    }


    
}

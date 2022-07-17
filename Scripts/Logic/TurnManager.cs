using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour {

  
    public static TurnManager Instance;

    public HourGlass timer_2;

    private Player _whoseTurn;
    public Player whoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;
            StartCoroutine("StartTurn");
           
            GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            TurnOwner tm = whoseTurn.GetComponent<TurnOwner>();
            tm.OnTurnStart();
            if (tm is TurnOwnerPlayer)
            {
                timer_2.DisableButton = false;
                whoseTurn.HighlightPlayableCards();
            }
            whoseTurn.otherPlayer.HighlightPlayableCards();

        }
    }


    void Awake()
    {
        Instance = this;
      
    }

    void Start()
    {
         OnGameStart();
    }

    public void OnGameStart()
    {
       
        CardInLogic.CardsCreatedThisGame.Clear();
        UnitInLogic.UnitsCreatedThisGame.Clear();
        SpellInLogic.SpellsActive.Clear();

        foreach (Player p in Player.Players)
        {
            p.APThisTurn = 0;
            p.ApLeft = 0;
            p.NumberOfTurns = 0;
            p.PlayHero(p.PArea);
            p.TransmitInfoAboutPlayerToVisual();
            p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
        
        }

     
                int rnd = Random.Range(0, 2);  
                Player whoGoesFirst = Player.Players[rnd];
                Player whoGoesSecond = whoGoesFirst.otherPlayer;

                // draw 4 cards for first player and 5 for second player
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {
                    // second player draws a card
                    whoGoesSecond.DrawACard(true);
                    // first player draws a card
                    whoGoesFirst.DrawACard(true);
                }
                // add one more card to second player`s hand
                whoGoesSecond.DrawACard(true);
            
                new StartATurnCommand(whoGoesFirst).AddToQueue();
         
    }


    public void EndTurn()
    {
        timer_2.EndTurn();
        whoseTurn.OnTurnEnd();

        new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
        timer_2.DisableButton = true;
    }

   

    IEnumerator StartTurn()
    {
        yield return new WaitForSeconds(3);
        timer_2.StartTurn();
        
    }

    public void StopTimer()
    {
        timer_2.StopTimer();
    }
    
}


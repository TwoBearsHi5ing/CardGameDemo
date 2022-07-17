using UnityEngine;
using System.Collections;

public class TurnOwnerPlayer : TurnOwner 
{
    public override void OnTurnStart()
    {
        //Debug.Log("Player turn Maker");
        base.OnTurnStart();
        // dispay a message that it is player`s turn
        new ShowMessageCommand("Your Turn!", 3f).AddToQueue();
        PlayerOwner.DrawACard();
    }
}

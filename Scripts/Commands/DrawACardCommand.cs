using UnityEngine;
using System.Collections;

public class DrawACardCommand : Command {

    private Player p;
    private CardInLogic cl;
    private bool fast;
    private bool fromDeck;

    public DrawACardCommand(CardInLogic cl, Player p, bool fast, bool fromDeck)
    {        
        this.cl = cl;
        this.p = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }

    public override void StartCommandExecution()
    {
        p.PArea.PDeck.CardsInDeck--;
        p.PArea.handVisual.GivePlayerACard(cl._cardAsset, cl.UniqueCardID, fast, fromDeck);
    }
}

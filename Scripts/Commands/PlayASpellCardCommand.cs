using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private CardInLogic card;
    private Player p;

    public PlayASpellCardCommand(Player p, CardInLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        p.PArea.handVisual.PlayASpellFromHand(card.UniqueCardID);
    }
}

using UnityEngine;
using System.Collections;

public class PlayACreatureCommand : Command
{
    private CardInLogic cl;
    private int tablePos;
    private Player p;
    private int creatureID;

    public PlayACreatureCommand(CardInLogic cl, Player p, int tablePos, int creatureID)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.creatureID = creatureID;
    }

    public override void StartCommandExecution()
    {
        HandVisual PlayerHand = p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        HoverPreview.PreviewsAllowed = true;
        p.PArea.DualTableVisual.AddCreatureAtIndex(cl._cardAsset, creatureID, tablePos, p.ID);
    }
}

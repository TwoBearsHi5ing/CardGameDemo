using UnityEngine;
using System.Collections;

public class PlayHeroCommand : Command
{
    private CardInLogic cl;
    private int tablePos;
    private Player p;
    private int creatureID;

    public PlayHeroCommand(CardInLogic cl, Player p, int tablePos, int creatureID)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.creatureID = creatureID;
    }

    public override void StartCommandExecution()
    {
        
        HoverPreview.PreviewsAllowed = false;
        p.PArea.DualTableVisual.AddHeroAtIndex(cl._heroAsset, creatureID, tablePos, p.ID);
    }
}

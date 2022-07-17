using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayASpellOnTableCommand : Command
{

    private CardInLogic cl;
    private Player p;
    private SpellInLogic sp;
    private int tabelPos;
    private int ID;

    public PlayASpellOnTableCommand(Player p, CardInLogic cl, SpellInLogic sl, int pos, int id )
    {
        this.cl = cl;
        this.p = p;
        sp = sl;
        tabelPos = pos;
        ID = id;
    }

    public override void StartCommandExecution()
    {

        HandVisual PlayerHand = p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        HoverPreview.PreviewsAllowed = true;



        p.PArea.DualTableVisual.PlaySpellOnTable(ID, tabelPos, cl._cardAsset,p.ID);
    }



}

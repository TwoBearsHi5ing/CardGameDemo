using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellExpireCommand : Command
{
    private Player p;
    private int ExpiredSpellID;
    private int ExpiredSpellPosition;

    public SpellExpireCommand(int spellID, Player owner, int index)
    {
        p = owner;
        ExpiredSpellID = spellID;
        ExpiredSpellPosition = index;
    }

    public override void StartCommandExecution()
    {
        p.PArea.DualTableVisual.RemoveCreatureWithID(ExpiredSpellID, ExpiredSpellPosition);
    }
}

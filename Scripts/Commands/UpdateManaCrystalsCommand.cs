using UnityEngine;
using System.Collections;

public class UpdateManaCrystalsCommand : Command {

    private Player p;
    private int TotalMana;
    private int AvailableMana;

    public UpdateManaCrystalsCommand(Player p, int TotalMana, int AvailableMana)
    {
        this.p = p;
        this.TotalMana = TotalMana;
        this.AvailableMana = AvailableMana;
    }

    public override void StartCommandExecution()
    {
        p.PArea.AP_Bar.Total_AP = TotalMana;
        p.PArea.AP_Bar.Available_AP = AvailableMana;
        CommandExecutionComplete();
    }
}

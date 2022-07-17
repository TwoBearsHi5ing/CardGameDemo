using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private GameObject looser;

    public GameOverCommand(GameObject looser)
    {
        this.looser = looser;
    }

    public override void StartCommandExecution()
    {
        looser.GetComponent<OneHeroManager>().Explode();
    }
}

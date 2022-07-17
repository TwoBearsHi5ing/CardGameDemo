using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum AreaPosition{Top, Low}

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition owner;
    public bool ControlsON = true;
    public PlayerDeckVisual PDeck;
    public ApPoolVisual AP_Bar;
    public HandVisual handVisual;

    public OneHeroManager oneHeroManager;

    public TableVisual DualTableVisual;
    public Transform PortraitPosition;
    public Transform InitialPortraitPosition;
    public Text testNumberOfTurns;

    public bool AllowedToControlThisPlayer
    {
        get;
        set;
    }      


}

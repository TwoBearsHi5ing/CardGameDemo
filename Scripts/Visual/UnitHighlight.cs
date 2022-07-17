using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHighlight : MonoBehaviour
{
    public Image cardGlow;
    public Color32 playerColor = new Color32(11, 238, 255, 255);
    public Color32 EnemyColor = new Color32(255, 11, 22, 255);
    int id;

    void Start()
    {
        id = GetComponent<IDHolder>().UniqueID;
        UnitInLogic cl = UnitInLogic.FindUnitLogicByID(id);

        if (cl.owner == Player.Players[0])
        {
            cardGlow.color = EnemyColor;
        }
        else if (cl.owner = Player.Players[1])
        {
            cardGlow.color = playerColor;
        }
    }
}

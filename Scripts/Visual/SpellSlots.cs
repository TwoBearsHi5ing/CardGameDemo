using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlots : MonoBehaviour
{
    public GameObject[] All_Spell_Slots;
    public static SpellSlots Instance;

   

    public Image[] AllSlotsBackgroung;
    public Image[] AllSlotsFrame;


    Vector3 mousePos;
    private void Awake()
    {
        Instance = this;
    }
   

    public void ChangeSlotColor(bool avaliable, List<int> Slots)
    {
        foreach (int i in Slots)
        {

            if (!avaliable)
            {
                AllSlotsBackgroung[i].color = new Color32(205, 0, 0, 90);

            }
            else if (avaliable)
            {
                AllSlotsBackgroung[i].color = new Color32(88, 219, 68, 90);

            }
        }
    }

    public void ChangeFrameColor(bool avaliable, List<int> Slots)
    {
        foreach (int i in Slots)
        {

            if (!avaliable)
            {
                AllSlotsFrame[i].color = new Color32(205, 0, 0, 255);
            }
            else if (avaliable)
            {
                AllSlotsFrame[i].color = new Color32(55, 222, 225, 255);
            }
        }
    }



    public void ReturnDefaultSlotColor(List<int> Slots)
    {
        foreach (int i in Slots)
        {
            AllSlotsBackgroung[i].color = new Color32(0, 0, 0, 90);

        }
    }

    public void ReturnDefaulFrameColor(List<int> Slots)
    {
        foreach (int i in Slots)
        {

            AllSlotsFrame[i].color = new Color32(255, 255, 225, 255);
        }
    }
}

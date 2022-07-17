using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabeSlots : MonoBehaviour
{
    public GameObject[] All_Creature_slots;
    public static TabeSlots Instance;

    public Image slotGlow;
    public GameObject slot;

    public Image[] AllSlotsBackgroung;
    public Image[] AllSlotsFrame;


    public int[] SlotsToHighlight;

  

    private void Awake()
    {
        Instance = this;
    }
    public int[] AssignSlotsToHighlight(int range, int[] values)
    {
        SlotsToHighlight = new int[range];

        for (int i = 0; i < range; i++)
        {
            SlotsToHighlight[i] = values[i];
        }

        return SlotsToHighlight;
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
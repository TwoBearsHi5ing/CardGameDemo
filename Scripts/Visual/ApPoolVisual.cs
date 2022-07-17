using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ApPoolVisual : MonoBehaviour
{

    public int MAX_AP = 10;

    public Text ProgressText;

    private int total_AP;
    public int Total_AP
    {
        get { return total_AP; }

        set
        {
            if (value > MAX_AP)
                total_AP = MAX_AP;
            else if (value < 0)
                total_AP = 0;
            else
                total_AP = value;
           
            ProgressText.text = string.Format("{0}/{1}", available_AP.ToString(), total_AP.ToString());
        }
    }

    private int available_AP;
    public int Available_AP
    {
        get { return available_AP; }

        set
        {
            if (value > total_AP)
                available_AP = total_AP;
            else if (value < 0)
                available_AP = 0;
            else
                available_AP = value;
        
            ProgressText.text = string.Format("{0}/{1}", available_AP.ToString(), total_AP.ToString());

        }
    }




    

}

using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
public class BetterCardRotation : MonoBehaviour
{

    
    public RectTransform CardFront;

    public RectTransform CardBack;


   
    void Update()
    {     
        if (isCardFrontFacingCamera())
        {
            CardFront.gameObject.SetActive(true);
            CardBack.gameObject.SetActive(false);
        }
        else
        {
            CardFront.gameObject.SetActive(false);
            CardBack.gameObject.SetActive(true);
        }
    }

    bool isCardFrontFacingCamera()
    {
        return Vector3.Dot(
            CardFront.transform.forward,
            Camera.main.transform.position - CardFront.transform.position) < 0;
    }
}



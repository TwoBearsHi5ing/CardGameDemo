using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MessageManager : MonoBehaviour
{
    public Text MessageText;
    public GameObject MessagePanel;

    public static MessageManager Instance;

    

    Vector3 point1 = new Vector3(-25, 0, 0);

    Vector3 point2 = new Vector3(0, 0, 0);

    Vector3 point3 = new Vector3(25, 0, 0);

    Vector3[] vector3s = new Vector3[2] { new Vector3 (-25,0,0), new Vector3 (25,0,0)};


    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
        MessagePanel.transform.position = point1;
    }

    public void ShowMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
      
    }

    IEnumerator ShowMessageCoroutine(string Message, float Duration)
    {
        Vector3 point1 = new Vector3(-25, 0, 0);
        Vector3 point2 = new Vector3(0, 0, 0);
        Vector3 point3 = new Vector3(25, 0, 0);

        MessageText.text = Message;
        MessagePanel.SetActive(true);
        MessagePanel.transform.position = point1;
        MessagePanel.transform.DOMove(point2, Duration/4);

        yield return new WaitForSeconds(Duration/3);

        Command.CommandExecutionComplete();
        MessagePanel.transform.DOMove(point3, Duration / 4);

        yield return new WaitForSeconds(Duration);    

        MessagePanel.SetActive(false);
       
       
    }

  
}

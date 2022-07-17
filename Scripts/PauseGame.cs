using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    void Update()
    {
        if (!GlobalSettings.Instance.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool tak = GlobalSettings.Instance.GameOverPanel.activeInHierarchy;

                if (tak)
                {
                    Time.timeScale = 1;
                    GlobalSettings.Instance.GameOverPanel.SetActive(false);
                }
                else if (!tak)
                {
                    Time.timeScale = 0;
                    GlobalSettings.Instance.GameOverPanel.SetActive(true);
                }


            }
        }
    }
}

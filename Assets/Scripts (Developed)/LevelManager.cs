using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject popUp;
    LevelEnd1 levelEnd1;
    public void SelectScene()
    {
        switch (this.gameObject.name)
        {
            case "StartButton":
                SceneManager.LoadScene("Level1");
                break;
            case "HowToPlay":
                popUp.SetActive(true);
                break;
            case "Close":
                popUp.SetActive(false);
                break;
            case "QuitButton":
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit(); ;
                break;
            case "RestartButton":
                /* if (levelEnd1.level1Completed)
                 {
                     SceneManager.LoadScene("Level3");
                 }
                 else
                 {
                     SceneManager.LoadScene("Level1");
                 } */
                SceneManager.LoadScene("Level1");
                break;
            case "MainMenuButton":
                SceneManager.LoadScene("StartScene");
                break;
        }
    }

}


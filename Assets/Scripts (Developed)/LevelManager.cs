using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject popUp;
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
            case "MainMenuButton":
               SceneManager.LoadScene("StartScreen");
               break;
        }
    }

}

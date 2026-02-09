using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void SelectScene()
    {
        switch (this.gameObject.name)
        {
            case "StartButton":
                SceneManager.LoadScene("Level1");
                break;
            case "QuitButton":
               SceneManager.LoadScene("LoseScene");
               break;
            case "MainMenuButton":
               SceneManager.LoadScene("StartScreen");
               break;
        }
    }

}

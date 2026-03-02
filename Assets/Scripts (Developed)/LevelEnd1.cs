using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd1 : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider ChangeScene) 
    {
        if (ChangeScene.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level3");
        }
    }
}

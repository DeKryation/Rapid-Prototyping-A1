using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MobileHealthController : MonoBehaviour
 {
    public Transform player;

    
    public float playerHealth = 100;
    [SerializeField] private Text healthText;

    private void Start()
    {
        UpdateHealth(); //update frm unity s
    }

    private void Update()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {

        healthText.text = playerHealth.ToString("0");
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }
}


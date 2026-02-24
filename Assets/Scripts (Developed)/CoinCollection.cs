using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            //Insert coin collection sfx    
            GameManager.coins++;
            Destroy(other.gameObject);
        }
    }
}

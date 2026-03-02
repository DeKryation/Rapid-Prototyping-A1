using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollection : MonoBehaviour
{
    public RawImage key1;
    public RawImage key2;
    private int keyNumber = 0;

    private void OnTriggerEnter(Collider other)
    {
        Color key1Color = key1.color;
        Color key2Color = key2.color;
        if (other.gameObject.CompareTag("Coin"))
        {
            //Insert coin collection sfx    
            AudioManager.Instance.PlaySFX(GameSFX.key);

            GameManager.coins++;
            Destroy(other.gameObject);
            if (keyNumber == 0)
            {
                key1Color.a = 1f;
                key1.color = key1Color;
                keyNumber++;
            }
            else if (keyNumber == 1)
            {
                key2Color.a = 1f;
                key2.color = key2Color;
            }
        }
    }
}

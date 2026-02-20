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
    [SerializeField] private Image flashImage; // Assign your vignette UI Image here

    [Header("Flash Red Settings")]
    [SerializeField] private float flashDuration = 0.4f;
    [SerializeField] private float minFlashAlpha = 0.1f;  // Flash starts at this alpha
    [SerializeField] private float maxFlashAlpha = 0.3f;  // Flash peaks at this alpha
    private float previousHealth;
    private Coroutine flashCoroutine;

    private void Start()
    {
        previousHealth = playerHealth;

        if (flashImage != null)
        {
            flashImage.color = new Color(1f, 0f, 0f, 0f);
        }
        UpdateHealth(); //update frm unity s
    }

    private void Update()
    {
        if (playerHealth < previousHealth)
        {
            TriggerFlash();
        }
        previousHealth = playerHealth;

        UpdateHealth();
    }
    public void UpdateHealth()
    {

        healthText.text = playerHealth.ToString("0");
        healthText.color = playerHealth < 10 ? Color.red : Color.white;

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }
    private void TriggerFlash()
    {
        if (flashImage == null) return;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float halfDuration = flashDuration / 2f;
        float elapsed = 0f;

        // Fade in from min to max
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(minFlashAlpha, maxFlashAlpha, elapsed / halfDuration);
            flashImage.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        elapsed = 0f;

        // Fade out from max back to 0
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(maxFlashAlpha, 0f, elapsed / halfDuration);
            flashImage.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        flashImage.color = new Color(1f, 0f, 0f, 0f);
        flashCoroutine = null;
    }
}


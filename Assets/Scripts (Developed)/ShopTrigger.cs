using UnityEngine;

public class ProximityCanvasTrigger : MonoBehaviour
{
    [Header("UI To Toggle")]
    [SerializeField] private GameObject canvasUI;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    [Header("Interaction Key")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("Optional: Prompt UI")]
    [SerializeField] private GameObject promptUI; // Optional "Press E to open shop" text

    private bool playerInRange = false;
    private bool isUIOpen = false;

    private void Awake()
    {
        // Safety: hide UI at start
        if (canvasUI != null)
            canvasUI.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        // Only check for input if player is in range
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ToggleUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;

            // Show prompt if assigned
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;

            // Hide prompt
            if (promptUI != null)
                promptUI.SetActive(false);

            // Auto-close UI if player walks away
            if (isUIOpen)
            {
                CloseUI();
            }
        }
    }

    private void ToggleUI()
    {
        if (isUIOpen)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    private void OpenUI()
    {
        if (canvasUI == null)
        {
            Debug.LogWarning($"[{name}] Canvas UI not assigned!");
            return;
        }

        canvasUI.SetActive(true);
        isUIOpen = true;

        // Hide prompt when UI is open
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void CloseUI()
    {
        if (canvasUI == null) return;

        canvasUI.SetActive(false);
        isUIOpen = false;

        // Show prompt again if player still in range
        if (playerInRange && promptUI != null)
            promptUI.SetActive(true);
    }
}
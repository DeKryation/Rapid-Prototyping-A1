using UnityEngine;

public class ProximityCanvasTrigger : MonoBehaviour
{
    [Header("UI To Toggle")]
    [SerializeField] private GameObject canvasUI;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    private void Awake()
    {
        // Safety: hide UI at start
        if (canvasUI != null)
            canvasUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            HideUI();
        }
    }

    private void ShowUI()
    {
        if (canvasUI == null)
        {
            Debug.LogWarning($"[{name}] Canvas UI not assigned!");
            return;
        }

        canvasUI.SetActive(true);
    }

    private void HideUI()
    {
        if (canvasUI == null) return;
        canvasUI.SetActive(false);
    }
}

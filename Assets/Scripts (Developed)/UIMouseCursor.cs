using UnityEngine;

public class UIMouseCursor : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0f; // Pause game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        Time.timeScale = 1f; // Resume game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }
}
using UnityEngine;

public class UIMouseCursor : MonoBehaviour
{
   
    void Start()
    {
        // Force cursor to show and unlock
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Keep cursor visible and unlocked (in case something tries to hide it)
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

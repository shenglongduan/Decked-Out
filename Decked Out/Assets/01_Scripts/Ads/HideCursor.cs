using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        if (!Application.isEditor)
        {
            // Hide the cursor
            Cursor.visible = false;
        }


        // Ensure the cursor is not locked
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Optional: You can toggle the cursor visibility with a key press for debugging purposes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the cursor visibility
            Cursor.visible = !Cursor.visible;
        }
    }
}

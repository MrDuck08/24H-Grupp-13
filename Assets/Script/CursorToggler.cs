using UnityEngine;

public class CursorToggler : MonoBehaviour
{
    [Header("Crosshair Setup")]
    // Drag your 'CrosshairUI' Image GameObject here in the Inspector
    public GameObject crosshairUIGameObject;

    // We no longer need to find crosshairController via GetComponent on CrosshairUIGameObject.
    // It's now on THIS GameObject, so we'll get it in Start().
    private CrosshairController crosshairController;

    private bool isCrosshairMode = false;

    void Start()
    {
        // Get the CrosshairController component, which is now on THIS GameObject
        crosshairController = GetComponent<CrosshairController>();
        if (crosshairController == null)
        {
            Debug.LogError("CursorToggler: Requires a CrosshairController script on the same GameObject!", this);
            enabled = false;
            return;
        }

        // Ensure the CrosshairUI GameObject itself is assigned
        if (crosshairUIGameObject == null)
        {
            Debug.LogError("CursorToggler: CrosshairUI GameObject is not assigned in the Inspector!", this);
            enabled = false;
            return;
        }

        // Initial state: Start with the regular mouse cursor visible
        SetMouseCursorMode();
    }

    public void ToggleCursorMode()
    {
        if (isCrosshairMode)
        {
            SetMouseCursorMode();
        }
        else
        {
            SetCrosshairMode();
        }
    }

    void SetCrosshairMode()
    {
        isCrosshairMode = true;

        // Hide the system cursor
        Cursor.visible = false;
        // Confine the cursor to the game window
        Cursor.lockState = CursorLockMode.Confined; // Or Locked if you want it centered

        // Activate the CrosshairUI GameObject
        if (crosshairUIGameObject != null)
        {
            crosshairUIGameObject.SetActive(true);
        }
        // Enable the CrosshairController script (which now moves the CrosshairUI)
        if (crosshairController != null)
        {
            crosshairController.enabled = true;
        }

        Debug.Log("Switched to Crosshair Mode.");
    }

    void SetMouseCursorMode()
    {
        isCrosshairMode = false;

        // Show the system cursor
        Cursor.visible = true;
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;

        // Deactivate the CrosshairUI GameObject
        if (crosshairUIGameObject != null)
        {
            crosshairUIGameObject.SetActive(false);
        }
        // Disable the CrosshairController script
        if (crosshairController != null)
        {
            crosshairController.enabled = false;
        }

        Debug.Log("Switched to Mouse Cursor Mode.");
    }

    void OnApplicationQuit()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        // Ensure cursor is normal if script is disabled (e.g., stopping play in editor)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
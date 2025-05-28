using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    // Public reference to the RectTransform of the CrosshairUI Image.
    // Drag your 'CrosshairUI' GameObject's RectTransform here in the Inspector!
    public RectTransform crosshairRectTransform;

    void Awake()
    {
        // Optional: Perform a check if the reference is not set in the Inspector
        if (crosshairRectTransform == null)
        {
            Debug.LogError("CrosshairController: 'Crosshair Rect Transform' is not assigned! Please assign the RectTransform of your CrosshairUI Image.", this);
            enabled = false; // Disable this script if it can't find its target
        }
        // Ensure the crosshair UI is initially hidden (redundant if CursorToggler handles it, but safe)
        if (crosshairRectTransform != null)
        {
            crosshairRectTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Only update position if crosshairRectTransform is assigned
        if (crosshairRectTransform != null)
        {
            // Set the position of the crosshair UI to the current mouse position.
            // Input.mousePosition gives screen coordinates, which RectTransform uses directly in Overlay mode.
            crosshairRectTransform.position = Input.mousePosition;
        }
    }
}
using UnityEngine;

public class GunRotation: MonoBehaviour
{
    // The main camera reference. Will try to find it automatically if not assigned.
    private Camera mainCamera;

    [Tooltip("Adjust this offset if your sprite isn't initially pointing right (0 degrees).")]
    [Range(-180f, 180f)]
    public float rotationOffset = -140f; // Use 0 if your gun sprite points right by default

    void Start()
    {
        // Try to find the main camera if not already assigned
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found! Please ensure your camera is tagged 'MainCamera'.", this);
            enabled = false; // Disable script if no camera
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        // 1. Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 2. Convert mouse screen position to world coordinates
        // We use the gun's Z position to ensure the mouse world position is on the same Z-plane.
        // This is important for correct 2D calculations, especially if the camera is not at Z=0.
        mouseScreenPosition.z = transform.position.z - mainCamera.transform.position.z; // Distance from camera to world point
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // 3. Calculate the direction vector from the gun to the mouse
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        // 4. Calculate the angle in degrees from the direction vector
        // Mathf.Atan2 returns the angle in radians between the x-axis and a 2D vector.
        // We convert it to degrees using Mathf.Rad2Deg.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 5. Apply the rotation to the gun's Z-axis, adjusting by the offset
        // Quaternion.Euler creates a rotation from Euler angles.
        // For 2D, we typically rotate around the Z-axis.
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }
}
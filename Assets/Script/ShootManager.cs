using UnityEngine;

public class ShootManager : MonoBehaviour
{
    [SerializeField] float destroyDistance = 100f; // How far the raycast will go

    private WaveSpawner waveSpawner;
    private SoundManager soundManager;

    void Start()
    {
       waveSpawner = FindAnyObjectByType<WaveSpawner>();
        soundManager = FindAnyObjectByType<SoundManager>();
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is for the left mouse button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        soundManager.PlaySound(soundManager.gunShotSound);

        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;

        // Convert the screen point to a world point (important for 2D raycasting)
        // This is where your ray will start in the 2D world
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

        // Perform the 2D raycast from the world point
        // Physics2D.Raycast returns a RaycastHit2D object if it hits something
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, destroyDistance); // Vector2.zero means no direction, just checking at a point

        // Check if the raycast hit anything
        if (hit.collider != null) // If 'hit.collider' is not null, something was hit
        {
            // Check if the hit object has the "Enemy" tag
            if (hit.collider.CompareTag("Enemy"))
            {
                soundManager.PlaySound(soundManager.hitEnemySound);
                Debug.Log("Hit an enemy: " + hit.collider.name);
                // Destroy the hit enemy GameObject
                var target = hit.collider.gameObject;
                target.GetComponent<KamikazeDroneScript>().DroneDestroyed();
                waveSpawner.EnemyDestroyed();
            }
            else
            {
                Debug.Log("Hit something else: " + hit.collider.name);
            }
        }
        else
        {
            soundManager.PlaySound(soundManager.hitSomthingElseSound);
            Debug.Log("Did not hit anything.");
        }
    }
}

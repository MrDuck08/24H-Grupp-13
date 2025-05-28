using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class CameraTurn : MonoBehaviour
{
    [SerializeField]
    Canvas presidentCanvas;

    [SerializeField]
    Canvas shootingCanvas;

    [SerializeField]
    float speed;

    [SerializeField]
    float zPosition;

    Vector2 wishPosition;
    bool presidentLook = true;

    DialogeSystem dialogeSystem;
    SoundManager soundManager;
    CursorToggler cursorToggler;

    private void Start()
    {
        // Set camera start position
        transform.position = presidentCanvas.transform.position;
        wishPosition = transform.position;

        soundManager = FindFirstObjectByType<SoundManager>();
        dialogeSystem = FindFirstObjectByType<DialogeSystem>();
        cursorToggler = FindFirstObjectByType<CursorToggler>();
    }

    private void Update()
    {
        Vector2 position2D = new Vector2(transform.position.x, transform.position.y);

        if ((wishPosition - position2D).magnitude < speed * Time.deltaTime * 1.01f)
        {
            transform.position = new Vector3(wishPosition.x, wishPosition.y, zPosition);
        } 
        else
        {
            Vector2 updatedposition2D = position2D;
            updatedposition2D += (wishPosition - position2D).normalized * speed * Time.deltaTime;
            transform.position = new Vector3(updatedposition2D.x, updatedposition2D.y, zPosition);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            presidentLook = !presidentLook;

            soundManager.PlaySound(soundManager.turnSound);
            cursorToggler.ToggleCursorMode();

            if (presidentLook == false)
            {
                dialogeSystem.DisableInput();
                onShootingShift();
            }
            else
            {
                dialogeSystem.EnableInput();
                onPresidentShift();
                dialogeSystem.skipIfTutorial();
            }
        }

    }

    /// <summary>
    /// Go to the shooting canvas
    /// </summary>
    public void onShootingShift()
    {
        wishPosition = shootingCanvas.transform.position;
    }

    /// <summary>
    /// Go to the president canvas
    /// </summary>
    public void onPresidentShift()
    {
        wishPosition = presidentCanvas.transform.position;
    }
}

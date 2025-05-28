using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseAndQuitManager : MonoBehaviour
{
    // A static boolean to easily check pause state from other scripts
    [SerializeField] static bool GameIsPaused = false;

    // Reference to your Pause Menu UI Panel
    [SerializeField] GameObject pauseMenuUI;

    [SerializeField] GameObject optionsCanvas;

    void Start()
    {
        // Ensure the pause menu is hidden at the start of the game
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        // Ensure game is not paused when starting
        GameIsPaused = false;
        Time.timeScale = 1f; // Set time to normal speed
        // Optional: Hide and lock cursor if you want it to be hidden during gameplay
        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/
    }

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && optionsCanvas.activeSelf == false)
            {
                Resume();
            }
            else if (optionsCanvas.activeSelf == false)
            {
                Pause();
            }
        }
    }

    // Call this method when the Resume button is clicked or Esc is pressed while paused
    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide the pause menu
        }
        Time.timeScale = 1f; // Set time back to normal speed (1 = real-time)
        GameIsPaused = false; // Update pause state
        // Optional: Hide and lock cursor again for gameplay
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Call this method when Esc is pressed during gameplay
    void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // Show the pause menu
        }
        Time.timeScale = 0f; // Stop time (0 = completely paused)
        GameIsPaused = true; // Update pause state
        // Optional: Make cursor visible and unlocked when paused
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Call this method when the Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        // This will only work in a build, not directly in the Unity Editor
        Application.Quit();

        // If in editor, you can stop playing:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

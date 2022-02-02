using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Written by: Randen Banuelos
// Based on Brackeys' implementation in his Menu tutorial

/// <summary>
/// The menu that shows up whenever a player hits their respective Pause button
/// </summary>
public class PauseMenu : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Static check for other systems to see if the game is paused
    /// </summary>
    public static bool GameIsPaused = false;


    /// <summary>
    /// Houses the Win Screen UI
    /// </summary>
    [SerializeField] private GameObject winMenuUI;

    /// <summary>
    /// Houses the Lose Screen UI
    /// </summary>
    [SerializeField] private GameObject loseMenuUI;

    /// <summary>
    /// Houses the Pause Screen UI
    /// </summary>
    [SerializeField] private GameObject pauseMenuUI;

    /// <summary>
    /// Button at the top of the Pause Screen
    /// </summary>
    [SerializeField] private Button pauseTopButton;

    /// <summary>
    /// Button at the top of the Win Screen
    /// </summary>
    [SerializeField] private Button winTopButton;

    /// <summary>
    /// Button at the top of the Lose Screen
    /// </summary>
    [SerializeField] private Button loseTopButton;

    /// <summary>
    /// Name of the Scene that has the Main Menu
    /// </summary>
    [SerializeField] private string mainMenuSceneName;

    /// <summary>
    /// Name of the Scene that has the Player Select Menu
    /// </summary>
    [SerializeField] private string playerSelectSceneName;


    // REFERENCES
    /// <summary>
    /// Input Actions that control the Menu UI
    /// </summary>
    private PauseControls controls;

    /// <summary>
    /// Cache for the GameManager instance for more efficient computing
    /// </summary>
    private GameManager manager;


    // FUNCTIONS
    private void Awake()
    {
        // Create a new instance of the PauseControls Input Actions
        controls = new PauseControls();

        // Cache GameManager
        manager = GameManager.Instance;
    } 

    /// <summary>
    /// Subscribes to pause-related delegates and the GameManager's state checks
    /// </summary>
    private void Start()
    {
        // Determine when to pause when the Input Actions fire a Pause command
        controls.Pause.PauseGame.started += _ => DeterminePause();

        // Subscribe to the GameManager's "lose state"
        manager.onAllPlayersDead += Lose;

        // Subscribe to the GameManager's "win state"
        manager.onAllBossesDead += Win;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    /// <summary>
    /// Determine if the game should currently be paused, based on GameIsPaused
    /// </summary>
    private void DeterminePause()
    {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    /// <summary>
    /// Freeze gameplay and enable the Lose screen
    /// </summary>
    private void Lose()
    {
        // Disable UI
        loseMenuUI.SetActive(true);

        // Freeze gameplay and set GameIsPaused
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Select button
        loseTopButton.Select();
    }

    /// <summary>
    /// Freeze gameplay and enable the Win screen
    /// </summary>
    private void Win()
    {
        // Disable UI
        winMenuUI.SetActive(true);

        // Freeze gameplay and set GameIsPaused
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Select button
        winTopButton.Select();
    }

    /// <summary>
    /// Freeze gameplay and enable the Pause screen
    /// </summary>
    private void Pause()
    {
        // Disable UI
        pauseMenuUI.SetActive(true);

        // Freeze gameplay and set GameIsPaused
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Select button
        pauseTopButton.Select();
    }

    /// <summary>
    /// Unfreeze gameplay and disable any UI
    /// </summary>
    public void Resume()
    {
        // Disable UI
        pauseMenuUI.SetActive(false);

        // Unfreeze gameplay and reset GameIsPaused
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// <summary>
    /// Unfreeze gameplay, reset/unsubscribe any managers, disable UI, and go to the Player Select screen
    /// </summary>
    public void LoadPlayerSelect()
    {
        // TODO: Combine this and LoadMainMenu()
        // Unfreeze gameplay
        Time.timeScale = 1f;

        // Reset managers
        GameManager.Instance.ResetGameManager();
        PlayerConfigurationManager.Instance.ResetSession();
        Inventory.Instance.ResetInventory();

        // Disable UI
        pauseMenuUI.SetActive(false);
        loseMenuUI.SetActive(false);
        GameIsPaused = false;

        // Unsubscribe
        controls.Pause.PauseGame.started -= _ => DeterminePause();
        manager.onAllPlayersDead -= Lose;
        manager.onAllBossesDead -= Win;

        // Load the Player Select screen
        SceneManager.LoadScene(playerSelectSceneName);
    }

    /// <summary>
    /// Unfreeze gameplay, reset/unsubscribe any managers, disable UI, and go to the Main Menu
    /// </summary>
    public void LoadMainMenu()
    {
        // Unfreeze gameplay
        Time.timeScale = 1f;

        // Reset managers
        GameManager.Instance.ResetGameManager();
        PlayerConfigurationManager.Instance.ResetSession();
        Inventory.Instance.ResetInventory();

        // Disable inputs joining
        PlayerConfigurationManager.Instance.DisableJoin();

        // Disable UI and reset GameIsPaused
        winMenuUI.SetActive(false);
        GameIsPaused = false;

        // Unsubscribe
        controls.Pause.PauseGame.started -= _ => DeterminePause();
        manager.onAllPlayersDead -= Lose;
        manager.onAllBossesDead -= Win;

        // Load the Main Menu
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        // Keep the Debug.Log for verification, since Quit() does not work in the Unity editor's player
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

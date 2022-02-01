using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject loseMenuUI;
    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private Button pauseTopButton;
    [SerializeField] private Button winTopButton;
    [SerializeField] private Button loseTopButton;

    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string playerSelectSceneName;

    private PauseControls controls;
    private GameManager manager;


    // FUNCTIONS

    private void Awake()
    {
        controls = new PauseControls();
        manager = GameManager.Instance;
    }


    private void OnEnable()
    {
        controls.Enable();
    }


    private void OnDisable()
    {
        controls.Disable();
    }


    private void Start()
    {
        controls.Pause.PauseGame.started += _ => DeterminePause();
        manager.onAllPlayersDead += Lose;
        manager.onAllBossesDead += Win;
    }


    private void DeterminePause()
    {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }


    private void Lose()
    {
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        loseTopButton.Select();
    }


    private void Win()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        winTopButton.Select();
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseTopButton.Select();
    }


    public void LoadPlayerSelect() // TODO: Combine this and LoadMainMenu()
    {
        Time.timeScale = 1f;

        GameManager.Instance.ResetGameManager();
        PlayerConfigurationManager.Instance.ResetSession();
        Inventory.Instance.ResetInventory();

        pauseMenuUI.SetActive(false);
        loseMenuUI.SetActive(false);
        GameIsPaused = false;

        controls.Pause.PauseGame.started -= _ => DeterminePause();
        manager.onAllPlayersDead -= Lose;
        manager.onAllBossesDead -= Win;

        SceneManager.LoadScene(playerSelectSceneName);
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        GameManager.Instance.ResetGameManager();
        PlayerConfigurationManager.Instance.ResetSession();
        Inventory.Instance.ResetInventory();

        PlayerConfigurationManager.Instance.DisableJoin();

        winMenuUI.SetActive(false);
        GameIsPaused = false;

        controls.Pause.PauseGame.started -= _ => DeterminePause();
        manager.onAllPlayersDead -= Lose;
        manager.onAllBossesDead -= Win;

        SceneManager.LoadScene(mainMenuSceneName);
    }


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

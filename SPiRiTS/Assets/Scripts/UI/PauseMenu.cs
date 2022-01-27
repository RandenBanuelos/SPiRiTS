using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private string menuSceneName;

    private PauseControls controls;


    // FUNCTIONS

    private void Awake()
    {
        controls = new PauseControls();
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
    }


    private void DeterminePause()
    {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }


    public void Resume()
    {
        Debug.Log("Resumed game!");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


    public void Pause()
    {
        Debug.Log("Paused game!");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void LoadMenu()
    {
        Debug.Log("Returning to player setup screen!");
        Time.timeScale = 1f;
        PlayerConfigurationManager.Instance.ResetSession();
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        SceneManager.LoadScene(menuSceneName);
    }


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private Button backButton;
    [SerializeField] private Button creditsButton;


    public void PlayGame()
    {
        PlayerConfigurationManager.Instance?.EnableJoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void GoToCredits()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
        backButton.Select();
    }


    public void GoBack()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        creditsButton.Select();
    }


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

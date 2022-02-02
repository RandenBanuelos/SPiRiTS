using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Written by: Randen Banuelos
// Based on Brackeys' implementation in his Menu tutorial

/// <summary>
/// The starting Menu for the game
/// </summary>
public class MainMenu : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Houses the Main Menu UI elements
    /// </summary>
    [SerializeField] private GameObject mainMenu;

    /// <summary>
    /// Houses the Credits Menu UI elements
    /// </summary>
    [SerializeField] private GameObject creditsMenu;

    /// <summary>
    /// Button that takes the player(s) back to the Main Menu
    /// </summary>
    [SerializeField] private Button backButton;

    /// <summary>
    /// Button that takes the player(s) to the Credits Menu
    /// </summary>
    [SerializeField] private Button creditsButton;


    // FUNCTIONS
    /// <summary>
    /// Enable joining and switch to the Player Select screen
    /// </summary>
    public void PlayGame()
    {
        PlayerConfigurationManager.Instance?.EnableJoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Switch menus to the Credits Menu, automatically selecting the Back button
    /// </summary>
    public void GoToCredits()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
        backButton.Select();
    }

    /// <summary>
    /// Switch back to the Main Menu, selecting the Credits button as default
    /// </summary>
    public void GoBack()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        creditsButton.Select();
    }

    /// <summary>
    /// Quit the game application
    /// </summary>
    public void QuitGame()
    {
        // Keep the Debug.Log for verification, since Quit() does not work in the Unity editor's player
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

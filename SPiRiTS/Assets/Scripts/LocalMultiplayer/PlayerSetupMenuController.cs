using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Written by: Randen Banuelos
// Based on Broken Knights Games' local multiplayer series using Unity's new Input System

/// <summary>
/// Controls an Instantiated Player Setup Menu prefab
/// </summary>
public class PlayerSetupMenuController : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Text that shows this player's number
    /// </summary>
    [SerializeField] private TextMeshProUGUI titleText;

    /// <summary>
    /// Color select menu
    /// </summary>
    [SerializeField] private GameObject menuPanel;

    /// <summary>
    /// Ready Up menu
    /// </summary>
    [SerializeField] private GameObject readyPanel;

    /// <summary>
    /// Ready Up button
    /// </summary>
    [SerializeField] private Button readyButton;

    /// <summary>
    /// Small icon to display what controller this player is using
    /// </summary>
    [SerializeField] private Image controllerIcon;

    /// <summary>
    /// Hosts all controller icons
    /// </summary>
    [SerializeField] private List<Sprite> icons;


    // REFERENCES
    /// <summary>
    /// Deadzone timing to prevent Unity's rapid fire issue
    /// </summary>
    private float ignoreInputTime = 0.5f;

    /// <summary>
    /// The cached player's index
    /// </summary>
    private int PlayerIndex;

    /// <summary>
    /// Used to temporarily disable player input
    /// </summary>
    private bool inputEnabled;


    // FUNCTIONS
    /// <summary>
    /// Give a small deadzone after joining to prevent rapid fire inputs from Unity's new Input System
    /// </summary>
    void Update()
    {
        if (!inputEnabled && Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    /// <summary>
    /// Set the player index to the menu, alongside setting the control scheme icon
    /// </summary>
    /// <param name="pi">This player's input index</param>
    /// <param name="scheme">The Input Actions mapping this player is using</param>
    public void SetPlayerIndex(int pi, string scheme)
    {
        // Set and show the player index
        PlayerIndex = pi;
        titleText.SetText("Player #" + (pi + 1).ToString());

        // Set the control scheme icon
        SetControllerIcon(scheme);

        // Set a small deadzone to prevent rapid fire
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    /// <summary>
    /// Set the small icon on the Setup Menu to show what type of controller the player is using
    /// </summary>
    /// <param name="scheme">Input Actions scheme that represents what mapping this player is using</param>
    private void SetControllerIcon(string scheme)
    {
        // TODO: Try using a switch statement instead of a cascade of if statements
        if (scheme == "Keyboard Mouse")
            controllerIcon.sprite = icons[0];
        if (scheme == "Switch Pro Controller")
            controllerIcon.sprite = icons[1];
        if (scheme == "PS4 Controller")
            controllerIcon.sprite = icons[2];
        if (scheme == "Xbox Controller" || scheme == "Generic Controller")
            controllerIcon.sprite = icons[3];
    }

    /// <summary>
    /// Set a color to this player, then move to the confirmation screen
    /// </summary>
    /// <param name="color">The chosen color</param>
    public void SetColor(Material color)
    {
        // Don't select anything if the deadzone is still up
        if (!inputEnabled) { return; }

        // Set this player's color in the overseeing PlayerConfigurationManager
        PlayerConfigurationManager.Instance.SetPlayerColor(PlayerIndex, color);

        // Enable the confirmation panel
        readyPanel.SetActive(true);
        readyButton.Select();

        // Disable the color select panel
        menuPanel.SetActive(false);
    }

    /// <summary>
    /// Ready up this player
    /// </summary>
    public void ReadyPlayer()
    {
        // Don't select anything if the deadzone is still up
        if (!inputEnabled) { return; }

        // Tell the PlayerConfigurationManager that this player is ready
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);

        // Disable the Ready Up button
        readyButton.gameObject.SetActive(false);
    }
}

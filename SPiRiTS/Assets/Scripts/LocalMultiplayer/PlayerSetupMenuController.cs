using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private Button readyButton;
    [SerializeField] private Image controllerIcon;
    [SerializeField] private List<Sprite> icons;

    private float ignoreInputTime = 0.5f;
    private bool inputEnabled;


    // REFERENCES
    private int PlayerIndex;


    // FUNCTIONS

    public void SetPlayerIndex(int pi, string scheme)
    {
        PlayerIndex = pi;
        titleText.SetText("Player #" + (pi + 1).ToString());
        SetControllerIcon(scheme);
        ignoreInputTime = Time.time + ignoreInputTime;
    }


    private void SetControllerIcon(string scheme)
    {
        if (scheme == "Keyboard Mouse")
            controllerIcon.sprite = icons[0];
        if (scheme == "Switch Pro Controller")
            controllerIcon.sprite = icons[1];
        if (scheme == "PS4 Controller")
            controllerIcon.sprite = icons[2];
        if (scheme == "Xbox Controller" || scheme == "Generic Controller")
            controllerIcon.sprite = icons[3];
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }


    public void SetColor(Material color)
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.SetPlayerColor(PlayerIndex, color);

        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }


    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }
}

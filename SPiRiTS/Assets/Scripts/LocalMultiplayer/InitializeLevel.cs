using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeLevel : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private MultipleTargetCamera cam;
    [SerializeField] private Transform mainCam;
    [SerializeField] private List<Canvas> huds = new List<Canvas>();
    [SerializeField] private List<Image> healthBubbles = new List<Image>();


    // FUNCTIONS

    // Start is called before the first frame update
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
            cam.AddTarget(player.transform);

            Inventory.Instance.AddNewPlayerInventory();

            Billboard healthBarBillboard = player.GetComponentInChildren<Billboard>();
            healthBarBillboard?.SetCamera(mainCam);

            Canvas hud = huds[i];
            Mover playerMover = player.GetComponent<Mover>();

            hud.gameObject.SetActive(true);
            playerMover.SetHUD(hud);
            playerMover.SetHealthBar(hud.GetComponentInChildren<HealthBar>());
            healthBubbles[i].color = playerConfigs[i].PlayerMaterial.color;
        }
    }
}

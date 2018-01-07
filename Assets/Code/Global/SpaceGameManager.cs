using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceGameManager : MonoBehaviour
{

    public enum GameMode
    {
        PRE_SETUP,
        SETUP,
        LOAD,
        NORMAL,
        EDITOR,
        MENU
    }

    public static SpaceGameManager instance;

    public GameMode Mode = SpaceGameManager.GameMode.PRE_SETUP;

    public GameObject DeathScreen;

    // @TODO: encapsulate this into a starting conditions struct.
    public List<GameObject> StartingShips;
    public List<GameObject> StartingModules;
    public int StartingScrap;
    public GameObject Destroyed;

    public void Start()
    {
        if (instance) {
            Debug.LogWarning("More than one space game manager");
            return;
        }
        instance = this;
    }

    public void SetGameMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.NORMAL:
                ShipEditor.instance.Disable();
                MenuController.instance.Disable();
                PlayerData.instance.PlayerShip.GetComponent<PlayerController>().enabled = true;
                HudController.instance.Enable();
                break;
            case GameMode.EDITOR:
                HudController.instance.Disable();
                MenuController.instance.Disable();
                PlayerData.instance.PlayerShip.GetComponent<PlayerController>().enabled = false;
                ShipEditor.instance.Enable();
                break;
            case GameMode.MENU:
                ShipEditor.instance.Disable();
                HudController.instance.Disable();
                PlayerData.instance.PlayerShip.GetComponent<PlayerController>().enabled = false;
                MenuController.instance.Enable();
                break;
        }
        this.Mode = mode;
    }

    public void Update()
    {
        switch (Mode)
        {
            case GameMode.SETUP:
                GameObject chosen = Utils.getRandomEntry<GameObject>(StartingShips);
                GameObject starter = GalaxyManager.SpawnWorldObject(chosen, Vector3.zero);
                GalaxyManager.instance.AddObserver(starter);
                starter.name = "Player";
                starter.AddComponent<PlayerController>();
                Ship ship = starter.GetComponent<Ship>();
                ship.SetFaction(FACTION.PLAYER_FACTION);
                ship.IsPlayer = true;
                PlayerData.instance.PlayerShip = ship;
                PlayerData.instance.AddScrap(StartingScrap);

                foreach (GameObject mod in StartingModules)
                {
                    PlayerData.instance.AddModule(mod);
                }

                CameraManager.instance.SetToFollow(PlayerData.instance.PlayerShip.gameObject);
                CameraManager.instance.SetOffset(new Vector3(0, 0, -10));
                HudController.instance.Disable();
                MenuController.instance.Disable();
                ShipEditor.instance.Enable();
                // Galaxy Generation
                Mode = GameMode.NORMAL;
                break;
        }
        HandleMetaInput();
    }

    public void PlayerDied() {
        DeathScreen.SetActive(true);
    }

    public void HandleMetaInput()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Mode == GameMode.EDITOR)
            {
                if (ShipChecker.instance.CheckShip())
                {
                    SetGameMode(GameMode.NORMAL);
                }
            }
            else
            {
                SetGameMode(GameMode.EDITOR);
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                CameraManager.instance.IncreaseSize();
            }
            else
            {
                CameraManager.instance.DecreaseSize();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Mode == GameMode.MENU)
            {
                SetGameMode(GameMode.NORMAL);
            }
            else {
                SetGameMode(GameMode.MENU);
            }
        }

    }

    public void ExitToDesktop() {
        Application.Quit();
    }

    public void MainMenu() {
        // @TODO: Load mm
        SceneManager.LoadScene(0);
    }

}

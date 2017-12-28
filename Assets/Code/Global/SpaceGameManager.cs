﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameManager : MonoBehaviour {

	public enum GameMode
	{
		PRE_SETUP,
		SETUP,
		LOAD,
		NORMAL,
		EDITOR
	}
		
	public GameMode Mode = SpaceGameManager.GameMode.PRE_SETUP;

	// @TODO: encapsulate this into a starting conditions struct.
	public List<GameObject> StartingShips;
	public List<GameObject> StartingModules;
	public int StartingScrap;

	public void SetGameMode(GameMode mode){
		switch (mode) {
		case GameMode.NORMAL:
			ShipEditor.instance.Disable ();
			break;
		case GameMode.EDITOR:
			ShipEditor.instance.Enable ();
			break;
		}

		this.Mode = mode;
	}

	public void Update() {
		switch (Mode) {
		case GameMode.SETUP:
			GameObject chosen = Utils.getRandomEntry<GameObject> (StartingShips);
			GameObject starter = Instantiate (chosen);
			Ship ship = starter.GetComponent<Ship> ();
			ship.SetFaction (FACTION.PLAYER_FACTION);
			ship.IsPlayer = true;
			PlayerData.instance.PlayerShip = ship;
			PlayerData.instance.AddScrap (StartingScrap);
			foreach (GameObject mod in StartingModules) {
				PlayerData.instance.AddModule (mod);		
			}

			CameraManager.instance.SetToFollow (PlayerData.instance.PlayerShip.gameObject);
			CameraManager.instance.SetOffset (new Vector3 (0, 0, -10));

			ShipEditor.instance.Disable ();

			// Galaxy Generation
			Mode = GameMode.NORMAL;
			break;
		}

		HandleMetaInput ();
	}

	public void HandleMetaInput() {

		if (Input.GetKeyDown (KeyCode.B)) {
			if (Mode == GameMode.EDITOR) {
				SetGameMode (GameMode.NORMAL);
			} else {
				SetGameMode(GameMode.EDITOR);
			}
		}

	}
}

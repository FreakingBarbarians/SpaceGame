using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RepairIcon : MonoBehaviour {
	private enum RepairState {
		IDLE,
		REPAIR,
		COMBAT
	}

	public Sprite RepairIdle;
	public Sprite RepairActive;
	public Sprite RepairCombat;

	public Image Icon;

	private float Intensity;
	int direction = 1;

	private RepairState state = RepairState.IDLE;

	void Update() {
		if (!PlayerData.instance.PlayerShip) {
			return;
		}

		Ship ship = PlayerData.instance.PlayerShip;

		switch (ship.State) {
		case Ship.ShipState.COMBAT:
			this.state = RepairState.COMBAT;
			break;
		case Ship.ShipState.REPAIR:
			this.state = RepairState.REPAIR;
			break;
		default:
			if (ship.DeltaPosition.magnitude != 0 || ship.DeltaRotation != 0) {
				this.state = RepairState.COMBAT;
			} else {
				this.state = RepairState.IDLE;
			}
			break;
		}

		switch (state) {
		case RepairState.IDLE:
			Icon.sprite = RepairIdle;
			Icon.color = new Color (1, 1, 1, 1);
			break;
		case RepairState.REPAIR:
			Icon.sprite = RepairActive;
			Icon.color = new Color (1, 1, 1, Intensity);

			if (Intensity >= 1) {
				direction = -1;
			}
			if (Intensity <= 0) {
				direction = 1;
			}

			Intensity += Time.deltaTime * direction;

			break;
		case RepairState.COMBAT:
			Icon.sprite = RepairCombat;
			Icon.color = new Color (1, 1, 1, 1);
			break;
		}

	}
}

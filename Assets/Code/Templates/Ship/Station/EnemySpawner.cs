using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class EnemySpawner : Station {
	public List<GameObject> ShipsToSpawn;
	public List<GameObject> SpawnedShips;
	public int MaxSpawned;
	// build time = repair time x 3

	private GameObject ChosenToBuild;
	private float Progress;

	protected override void tick ()
	{
		
		EnergyCur = Mathf.Min (EnergyMax, EnergyCur + EnergyRegen);
		switch (State) {
		case ShipState.REPAIR:
			Repair (SpaceGameGlobal.TICK_RATE);
			break;
		case ShipState.COMBAT:
			if (CombatCooldownTimer <= 0) {
				SetState (ShipState.NORMAL);
			} else {
				CombatCooldownTimer -= SpaceGameGlobal.TICK_RATE;
			}
			break;
		}

		if (State != ShipState.COMBAT) {
			Build ();
		}

	}

	private void Build() {
		// done building
		if (Progress <= 0) {
			// we are actually building sth
			if (ChosenToBuild) {
				// Create the ship
				GameObject newShip = GalaxyManager.SpawnWorldObject(ChosenToBuild, transform.position);
				Ship ship = newShip.GetComponent<Ship> ();

				// Set hp to 1/5
				ship.curhp = ship.maxhp / 5;

				foreach (Port p in ship.ports) {
					Module m;
					if ((m = p.GetModule ())) {
						m.curhp = m.maxhp / 5;
					}
				}

				foreach (Port p in ship.mainPorts) {
					Module m;
					if ((m = p.GetModule ())) {
						m.curhp = m.maxhp / 5;
					}
				}

				// set faction
				ship.SetFaction (this.faction);
				// add to list
				SpawnedShips.Add (newShip);

				ChosenToBuild = null;
			}

			if (SpawnedShips.Count < MaxSpawned) {
				ChosenToBuild = Utils.getRandomEntry<GameObject> (ShipsToSpawn);
				Progress = ChosenToBuild.GetComponent<Ship> ().RepairTime * 3;
			}
		} else {
			Progress -= SpaceGameGlobal.TICK_RATE;
		}
	}

	public override void HandleQvent (QventSystem.Qvent qvent)
	{
		switch (qvent.QventType) {
		case QventType.DAMAGED:
			SetState (ShipState.COMBAT);
			CombatCooldownTimer = SpaceGameGlobal.COMBAT_COOLDOWN;
			break;
		case QventType.DESTROYED:
			if (qvent.PayloadType.IsSubclassOf (typeof(Ship))) {
				SpawnedShips.Remove (((Ship)qvent.Payload).gameObject);
			}
			break;
		}
	}
}

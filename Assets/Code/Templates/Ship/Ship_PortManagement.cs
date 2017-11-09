using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Ship : Damageable{
	public void Refresh() {
		// recalculate
		// calculate health of the ship from main port module's

		int hpSum = 0;
		foreach (Port p in mainPorts) {
			if (p.IsConnected()) {
				Module mainMod =  p.GetModule();
				mainMod.RegisterShip(this);
			}
		}

		foreach (Port p in ports) {
			if (p.IsConnected ()) {
				Module mainMod = p.GetModule ();
				mainMod.RegisterShip (this);
			}
		}
	}

	public void OnPortDisabled(Module mod){
		energyRegen -= mod.energyRegen;
		energyMax -= mod.energyCap;
		energyCur = Mathf.Min(energyCur, energyMax);
		speedMax -= mod.thrustPower;
		speedCur = Mathf.Min(speedCur, speedMax);
	}

	public void OnPortEnabled(Module mod){
		energyRegen += mod.energyRegen;
		energyMax += mod.energyCap;
		energyCur += mod.energyCap;
		speedMax += mod.thrustPower;
		speedCur += mod.thrustPower;
	}

	public void OnMainPortConnected(Module mod) {
		maxhp += mod.maxhp;
		curhp += mod.curhp;
		energyRegen += mod.energyRegen;
		energyMax += mod.energyCap;
		energyCur += mod.energyCap;
		speedMax += mod.thrustPower;
		speedCur += mod.thrustPower;
		connectHelper (mod);
	}

	public void OnMainPortDisconnected(Module mod) {
		maxhp -= mod.maxhp;
		curhp = Mathf.Min (curhp, maxhp);
		energyRegen -= mod.energyRegen;
		energyMax -= mod.energyCap;
		energyCur = Mathf.Min(energyCur, energyMax);
		speedMax -= mod.thrustPower;
		speedCur = Mathf.Min(speedCur, speedMax);
		disconnectHelper (mod);
	}

	public void OnPortConnected(Module mod) {
		// stuff
		// update max stats
		energyRegen += mod.energyRegen;
		energyMax += mod.energyCap;
		energyCur += mod.energyCap;
		speedMax += mod.thrustPower;
		speedCur += mod.thrustPower;
		connectHelper (mod);
	}

	public void OnPortDisconnected(Module mod) {
		// more stuff
		energyRegen -= mod.energyRegen;
		energyMax -= mod.energyCap;
		energyCur = Mathf.Min(energyCur, energyMax);
		speedMax -= mod.thrustPower;
		speedCur = Mathf.Min(speedCur, speedMax);
		disconnectHelper (mod);
	}

	private void connectHelper(Module mod){
		if (mod.moduleType == Module.ModuleType.Weapon) {
			weapons.Add ((Weapon)mod);
		}
		mod.RegisterShip (this);
		mod.faction = faction;
	}

	private void disconnectHelper(Module mod){
		if (mod.moduleType == Module.ModuleType.Weapon) {
			weapons.Remove ((Weapon)mod);
		}
		mod.RegisterShip (this);
		mod.faction = FACTION.NEUTRAL_FACTION;
	}
}

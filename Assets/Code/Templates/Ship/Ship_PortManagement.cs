using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public partial class Ship : Damageable{

	public void Refresh() {
		// recalculate
		// calculate health of the ship from main port module's
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

    public void OnPortDisabled(Module mod) {
        moduleEnableHelper(mod, false);
    }

    public void OnPortEnabled(Module mod) {
        moduleEnableHelper(mod, true);
    }

	public void OnPortConnected(Module mod) {
        moduleConnectHelper(mod, true);
        moduleEnableHelper(mod, true, mod.portType == Port.PortType.MAIN);

		Qvent q = new Qvent (QventType.STRUCTURE_CHANGED);
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (q);
		}

		mod.RegisterListener (this);
    }

	public void OnPortDisconnected(Module mod) {
        moduleConnectHelper(mod, false);
        moduleEnableHelper(mod, false, mod.portType == Port.PortType.MAIN);

		Qvent q = new Qvent (QventType.STRUCTURE_CHANGED);
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (q);
		}

		mod.UnregisterListener (this);
	}

    private void moduleConnectHelper(Module mod, bool connecting) {
        switch (connecting) {
		case true:
			if (mod.moduleType == Module.ModuleType.WEAPON) {
				weapons.Add ((Weapon)mod);
			}
			mod.RegisterShip (this);
			mod.faction = faction;
			mod.RegisterListener (hpb);
                break;
            case false:
                if (mod.moduleType == Module.ModuleType.WEAPON)
                {
                    weapons.Remove((Weapon)mod);
                }
            mod.RegisterShip(null);
            mod.faction = FACTION.NEUTRAL_FACTION;
			mod.UnregisterListener (hpb);
                break;
        }

    }

    private void moduleEnableHelper(Module mod, bool addingBenefits, bool mainModule = false) {
        int mult = addingBenefits ? 1 : -1;

        EnergyRegen += mult * mod.EnergyRegen;
        EnergyMax += mult * mod.EnergyMax;
        EnergyCur = Mathf.Min(EnergyCur, EnergyMax);
        DeltaPositionFactor += mult * mod.DeltaPositionFactor;
        DeltaRotationMax += mult * mod.DeltaRotationMax;
        DeltaPositionFactor += mult * mod.DeltaPositionFactor;
        DeltaPositionMax += mult * mod.DeltaPositionMax;
        if (mainModule) {
            maxhp += mult * mod.maxhp;
            curhp = Mathf.Min(maxhp, curhp);
        }
    }

    // fix up my naming conventions...
}

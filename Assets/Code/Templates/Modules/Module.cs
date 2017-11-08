using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Module : Damageable {
	public enum ModuleType {
		Standard,
		Weapon
	}

    public Port.PortType portType;
	public ModuleType moduleType;
    public bool operational = true;
	protected Port root;
	protected Ship rootShip;

    public void Register (Port port) {
        this.root = port;
    }

	// needed?
    public void OnConnect() {
        // make CONNECTIONS
    }

	// probably not :/
    public void OnDisconnect() {
        // destroy CONNECTIONS
    }

	public void RegisterShip(Ship ship) {
		this.rootShip = ship;
	}

	public override void DoDamage (int amt)
	{
		this.curhp = Mathf.Max (curhp, 0);
		if (curhp == 0) {
			Die ();
		}
	}

	public override void DoHeal (int amt)
	{
		this.curhp += amt;
		if (operational == false) {
			// revive
		}
	}

	public override void Die()
	{
		operational = false;
		// other stuff
	}

}

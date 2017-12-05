using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

public partial class Module : Damageable {
    // Class specific stuff \\
    public enum ModuleType {
		Standard,
		Weapon
	}

    public static ModuleType StringToModuleType(string moduleType) {
        string s = moduleType.ToString().ToLower();
        if (ModuleType.Standard.ToString().ToLower().Equals(s))
        {
            return ModuleType.Standard;
        }
        else if (ModuleType.Weapon.ToString().ToLower().Equals(s)) {
            return ModuleType.Weapon;
        }

        return ModuleType.Standard;
    }

    // Fields n shit \\

    // serialize/deserialize these fields
    public Port.PortType portType;
	public ModuleType moduleType;
    public bool operational = true;
	public bool adrift = false;

    // these fields will be set when re-generating the ship
	protected Port root;
	protected Ship rootShip;
	protected Animator annie;

	void Start(){
		annie = GetComponent<Animator> ();
		if (annie == null) {
			annie = gameObject.AddComponent<Animator> ();
		}

	}

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
		annie.SetTrigger ("Damaged");
		this.curhp = Mathf.Max (curhp - amt, 0);

		if (portType == Port.PortType.MAIN) {
			rootShip.DoDamage (amt);
		}

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
		annie.SetBool ("Die",true);
		operational = false;
		gameObject.layer = LayerMask.NameToLayer ("Debris");
		rootShip.OnPortDisabled (this);
		// other stuff
	}

	public void SetAdrift(){
		this.adrift = true;

		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D> ();

		Vector2 away = transform.position - rootShip.transform.position;
		rb.velocity = away.normalized * Random.value;
		gameObject.layer = LayerMask.NameToLayer ("Debris");
		rb.gravityScale = 0;
		// @TODO: pick up logic & set logic
	}

    // provide serialization for "STANDARD TYPES" pass on later? hmm...
    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteElementString("PORT_TYPE", portType.ToString());
        writer.WriteElementString("MODULE_TYPE", moduleType.ToString());
        XmlUtils.SerializeBool(writer, operational, "OPERATIONAL");
        XmlUtils.SerializeBool(writer, adrift, "ADRIFT");
        writer.WriteElementString("ENERGY_CAP", energyCap.ToString());
        writer.WriteElementString("ENERGY_REGEN", energyRegen.ToString());
        writer.WriteElementString("THRUST_POWER", thrustPower.ToString());
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

public partial class Module : Damageable {
    // Class specific stuff \\
    public enum ModuleType {
		STANDARD,
		WEAPON,
        COCKPIT
	}

    public static ModuleType StringToModuleType(string moduleType) {
        string s = moduleType.ToString().ToLower();

        if (ModuleType.STANDARD.ToString().ToLower().Equals(s))
        {
            return ModuleType.STANDARD;
        }
        else if (ModuleType.WEAPON.ToString().ToLower().Equals(s)) {
            return ModuleType.WEAPON;
        }

        return ModuleType.STANDARD;
    }

    // Fields n shit \\

	// this will be saved in prefab so no need to serialize :D
	public int ScrapCost;

	[Range(1,100)]
	public int RarityMult = 100;

    // serialize/deserialize these fields
    public Port.PortType portType;
	public ModuleType moduleType;
    public bool operational = true;
	public bool adrift = false;

    // these fields will be set when re-generating the ship
    [HideInInspector]
	public Port root;
    [HideInInspector]
    public Ship rootShip;
	protected Animator annie;

	void Start(){
		Init ();
	}

	protected virtual void Init() {
		annie = GetComponent<Animator> ();
		if (annie == null) {
			annie = gameObject.AddComponent<Animator> ();
		}
		AddHealthBar ();
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
		base.DoDamage (amt);

		annie.SetTrigger ("Damaged");

		if (portType == Port.PortType.MAIN) {
			rootShip.DoDamage (amt);
		}

		if (curhp == 0) {
			Die ();
		}

	}

	public override void DoHeal (int amt)
	{
		base.DoHeal (amt);
		if (operational == false) {
			annie.SetBool ("Die", false);
			operational = true;
			if (rootShip) {
				rootShip.OnPortEnabled (this);
			}
			gameObject.layer = LayerMask.NameToLayer ("Module");
		}
	}

	public override void Die()
	{

		annie.SetBool ("Die",true);
		operational = false;
		if (portType == Port.PortType.MAIN) {
			
		} else {
			gameObject.layer = LayerMask.NameToLayer ("Debris");
		}
		if (rootShip) {
			rootShip.OnPortDisabled (this);
		}

        Sound s = SoundPool.instance.Get("EXPLOSION");
        s.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        s.gameObject.SetActive(true);
        s.Play();
	}

	public void SetAdrift(){
		this.adrift = true;

		float chance = (float)RarityMult + ((float)RarityMult / 2) * ((float)curhp / maxhp);

		GameObject debris;

		if (Utils.Rollf (chance, 100)) {
			debris = WidgetManager.instance.CreateFloatingSchematic (this, transform.position);
		} else {
			debris = WidgetManager.instance.CreateFloatingScrap ((int)(ScrapCost * (0.5f + Random.value/2)), transform.position);
		}

		Rigidbody2D rb = debris.GetComponent<Rigidbody2D> ();

		Vector2 away = transform.position - rootShip.transform.position;
		rb.velocity = away.normalized * Random.value;
		gameObject.layer = LayerMask.NameToLayer ("PlayerOnly");
		rb.gravityScale = 0;

		GameObject.Destroy (gameObject);
		// @TODO: pick up logic & set logic
	}

    // ReadXml
    public new static GameObject ReadXml(XmlReader reader, Component workingObj)
    {
        Module module = (Module)workingObj;

        reader.Read();
        module.portType = Port.StringToPortType(reader.ReadString());

        reader.Read();
        module.moduleType = Module.StringToModuleType(reader.ReadString());

        reader.Read();
        module.operational = XmlUtils.DeserializeBool(reader);

        reader.Read();
        module.adrift = XmlUtils.DeserializeBool(reader);

        reader.Read();
        module.EnergyMax = int.Parse(reader.ReadString());

        reader.Read();
        module.EnergyRegen = int.Parse(reader.ReadString());

        reader.Read();
        module.DeltaRotationMax = float.Parse(reader.ReadString());

        reader.Read();
        module.DeltaPositionFactor = float.Parse(reader.ReadString());

        reader.Read();
        module.DeltaRotationMax = float.Parse(reader.ReadString());

        reader.Read();
        module.DeltaRotationAcceleration = float.Parse(reader.ReadString());
        return workingObj.gameObject;
    }
    
    // provide serialization for "STANDARD TYPES" pass on later? hmm...
    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);

        writer.WriteStartElement("MODULE_DATA");
        writer.WriteElementString("PORT_TYPE", portType.ToString());
        writer.WriteElementString("MODULE_TYPE", moduleType.ToString());
        XmlUtils.SerializeBool(writer, operational, "OPERATIONAL");
        XmlUtils.SerializeBool(writer, adrift, "ADRIFT");
        writer.WriteElementString("ENERGY_CAP", EnergyMax.ToString());
        writer.WriteElementString("ENERGY_REGEN", EnergyRegen.ToString());
        writer.WriteElementString("DELTA_POSITION_MAX", DeltaPositionMax.ToString());
        writer.WriteElementString("DELTA_POSITION_FACTOR", DeltaPositionFactor.ToString());
        writer.WriteElementString("DELTA_ROTATION_MAX", DeltaRotationMax.ToString());
        writer.WriteElementString("DELTA_ROTATION_ACCELERATION", DeltaRotationAcceleration.ToString());

        writer.WriteEndElement();
    }
}

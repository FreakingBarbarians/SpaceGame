using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

public class ShotWeapon : Weapon {

    // serialize/deserialize all these
	[Range(0,10)]
	public float cooldown;

    public float cooldownTimer;

	[Range(0, 1000)]
	public int energyCost;

	public void FixedUpdate() {
		if (cooldownTimer > 0) {
			cooldownTimer -= Time.fixedDeltaTime;
		}
	}

    public new static GameObject ReadXml(XmlReader reader, Component workingObj) {
        ShotWeapon shotWeapon = (ShotWeapon)workingObj;

        reader.Read();
        shotWeapon.cooldown = float.Parse(reader.ReadString());

        reader.Read();
        shotWeapon.cooldownTimer = float.Parse(reader.ReadString());

        reader.Read();
        shotWeapon.energyCost = int.Parse(reader.ReadString());
        return workingObj.gameObject;
    }

    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteStartElement("SHOT_WEAPON");

        writer.WriteElementString("COOLDOWN", cooldown.ToString());
        writer.WriteElementString("COOLDOWN_TIMER", cooldownTimer.ToString());
        writer.WriteElementString("ENERGY_COST", energyCost.ToString());

        writer.WriteEndElement();
    }
}

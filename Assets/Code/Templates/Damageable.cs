using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;

using QventSystem;

public class Damageable : MyPrefab, IQventEmitter {
	
	// QventEmitter Interface Declaration

	// @TODO: Please generate this at runtime. ahhhhhhh
	[NonSerialized]
	public List<QventHandler> Listeners = new List<QventHandler> ();

	public void RegisterListener (QventHandler Listener) {
		if (!Listeners.Contains (Listener)) {
			Listeners.Add (Listener);
		}
	}

	public void UnregisterListener (QventHandler Listener) {
		Listeners.Remove (Listener);
	}

	[Range(1,360)]
	public float RepairTime = 10;

	public FACTION faction;

    [Range (0, 9999)]
    public int maxhp;

    [Range(0, 9999)]
    public int curhp;

    public bool invincible = false;

	public virtual void Repair(float time){
		int repamt = Mathf.Max((int) ((time / RepairTime)*maxhp), 1);
		DoHeal (repamt);

		Qvent q = new Qvent (QventType.HEALED);
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (q);
		}
	}

	protected virtual void AddHealthBar() {
		GameObject hpBar = WidgetManager.instance.CreateHealthBar ();
		hpBar.transform.position = transform.position;
		hpBar.transform.SetParent (transform);
		HealthBar bar = hpBar.GetComponent<HealthBar> ();
		bar.source = this;
		RegisterListener (bar);
		bar.Refresh ();

		Sprite s = GetComponent<SpriteRenderer> ().sprite;
		float yoffset =  s.textureRect.size.y / s.pixelsPerUnit;
		hpBar.transform.position += new Vector3 (0, -yoffset/2, 0);
	}

    public virtual void DoDamage(int amt) {
        if (invincible || amt < 0) {
            return;
        }

        curhp = (curhp - amt <= 0)? 0 : curhp - amt;

		Qvent q = new Qvent (QventType.DAMAGED);
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (q);
		}

        if (curhp <= 0) {
            Die();
        }
    }

    public virtual void DoHeal(int amt) {
        curhp = (curhp + amt > maxhp) ? maxhp : curhp + amt;

		Qvent q = new Qvent (QventType.HEALED);
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (q);
		}

    }

    public virtual void Die() {
        // to be overridden
        throw new NotImplementedException("Function Die in " + this.GetType().Name + " not Implemented");
    }

    public new static GameObject ReadXml(XmlReader reader, Component workingObj)
    {
        Damageable damageable = (Damageable)workingObj;

        reader.Read();
        damageable.faction = SpaceGameGlobal.StringToFaction(reader.ReadString());

        reader.Read();
        damageable.maxhp = int.Parse(reader.ReadString());

        reader.Read();
        damageable.curhp = int.Parse(reader.ReadString());

		reader.Read ();
		damageable.RepairTime = float.Parse (reader.ReadString ());

        return workingObj.gameObject;
    }

    public override void WriteXml(XmlWriter writer) {
        base.WriteXml(writer);
        writer.WriteStartElement("DAMAGEABLE");

        writer.WriteElementString("FACTION", faction.ToString());
        writer.WriteElementString("MAX_HP", maxhp.ToString());
        writer.WriteElementString("CUR_HP", curhp.ToString());
		writer.WriteElementString ("REPAIR_TIME", RepairTime.ToString ());

        writer.WriteEndElement();
    }

}

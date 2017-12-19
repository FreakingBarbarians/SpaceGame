﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;

public class Damageable : MyPrefab {

	public FACTION faction;

    [Range (0, 9999)]
    public int maxhp;

    [Range(0, 9999)]
    public int curhp;

    public bool invincible = false;

    public virtual void DoDamage(int amt) {

        if (invincible || amt < 0) {
            return;
        }

        curhp = (curhp - amt <= 0)? 0 : curhp - amt;

        if (curhp <= 0) {
            Die();
        }
    }

    public virtual void DoHeal(int amt) {
        curhp = (curhp + amt > maxhp) ? maxhp : curhp + amt;
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

        return workingObj.gameObject;
    }

    public override void WriteXml(XmlWriter writer) {
        base.WriteXml(writer);
        writer.WriteStartElement("DAMAGEABLE");

        writer.WriteElementString("FACTION", faction.ToString());
        writer.WriteElementString("MAX_HP", maxhp.ToString());
        writer.WriteElementString("CUR_HP", curhp.ToString());

        writer.WriteEndElement();
    }

}

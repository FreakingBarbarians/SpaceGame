using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Weapon : Module {
    // Look at this coolness!

    public enum WeaponType {
        BASE,
        SINGLE_SHOT_WEAPON
    }

    public static WeaponType StringToWeaponType(string weaponType)
    {
        string s = weaponType.ToString().ToLower();

        if (WeaponType.SINGLE_SHOT_WEAPON.ToString().ToLower().Equals(s))
        {
            return SingleShotWeapon.WeaponType.SINGLE_SHOT_WEAPON;
        }

        return WeaponType.SINGLE_SHOT_WEAPON;
    }

    // serialize/deserialize these
    [HideInInspector]
    public WeaponType weaponType = WeaponType.BASE;
    [HideInInspector]
    public int WeaponMask = 1;

    public virtual void UpdateWeaponState(int WeaponMask) {
        if ((this.WeaponMask & WeaponMask) != 0) {
            // fire
            Debug.Log("FIRE");
        } else {
            // stop fire
            Debug.Log("STOP FIRE");
        }
    }

    public virtual void PointTowards(Vector3 point) {
        point = new Vector3(point.x, point.y, transform.position.z);
        Debug.DrawLine(transform.position, point);
        transform.up = Vector3.RotateTowards(transform.up, point - transform.position, Mathf.Infinity, 0);
    }

    public new static GameObject ReadXml(XmlReader reader, Component workingObj) {
        Weapon weapon = (Weapon)workingObj;

        reader.Read();
        weapon.weaponType = Weapon.StringToWeaponType(reader.ReadString());

        reader.Read();
        weapon.WeaponMask = int.Parse(reader.ReadString());

        return workingObj.gameObject;
    }

    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);

        writer.WriteStartElement("WEAPON");

        writer.WriteElementString("WEAPON_TYPE", weaponType.ToString());
        writer.WriteElementString("WEAPON_MASK", WeaponMask.ToString());

        writer.WriteEndElement();
    }
}
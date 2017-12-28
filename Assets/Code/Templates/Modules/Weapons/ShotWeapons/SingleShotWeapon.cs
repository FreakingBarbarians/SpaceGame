using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class SingleShotWeapon : ShotWeapon {
	
	public GameObject bulletPrefab;
	public GameObject Muzzle;

    [Range(1, 9999)]
    public int Damage;

    [Range(1, 9999)]
    public float Velocity;

	private Bullet bulletComponent;

	public bool released = true;

	protected override void Init ()
	{
		base.Init ();
		bulletComponent = bulletPrefab.GetComponent<Bullet> ();

		bulletComponent.damage = Damage;
		bulletComponent.velocity = Velocity;

		annie.speed = 1f / cooldown;
	}

	public new void FixedUpdate(){
		if (cooldownTimer > 0 && released) {
			cooldownTimer -= Time.fixedDeltaTime;
		}
	}

	public override void UpdateWeaponState(int WeaponMask){
		if (!operational) {
			return;
		}

		if ((this.WeaponMask & WeaponMask) != 0) {
			// fire
			if(released && cooldownTimer <= 0){
				if (rootShip.EnergyCur < energyCost) {
					return;
				}

				released = false;
				cooldownTimer = cooldown;
				annie.SetTrigger ("Fire");
				annie.SetBool ("Released", false);

				Bullet bullet = BulletPoolManager.instance.Get(bulletComponent.BASE_NAME);

				if (bullet == null) {
					return;
				}

				// orient bullet
				bullet.faction = rootShip.faction;
				bullet.transform.position = Muzzle.transform.position;
				bullet.transform.rotation = Muzzle.transform.rotation;

                bullet.damage = this.Damage;
                bullet.velocity = this.Velocity;

				// activate bullet
				bullet.gameObject.SetActive (true);

				rootShip.EnergyCur -= energyCost;
			}
		} else {
			// stop fire
			annie.SetBool("Released", true);
			released = true;
		}
	}

    public new static GameObject ReadXml(XmlReader reader, Component workingObj) {
        SingleShotWeapon ssw = (SingleShotWeapon)workingObj;

        reader.Read();
        ssw.Damage = int.Parse(reader.ReadString());

        reader.Read();
        ssw.Velocity = float.Parse(reader.ReadString());

        return workingObj.gameObject;
    }


    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);

        writer.WriteStartElement("SINGLE_SHOT_WEAPON");

        writer.WriteElementString("DAMAGE", Damage.ToString());
        writer.WriteElementString("VELOCITY", Velocity.ToString());

        writer.WriteEndElement();
    }
}

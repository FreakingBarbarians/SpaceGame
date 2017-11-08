﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : ShotWeapon {
	
	public GameObject Bullet;
	public GameObject Muzzle;

	public bool released = true;

	public override void UpdateWeaponState(int WeaponMask){
		if (!operational) {
			return;
		}

		if ((this.WeaponMask & WeaponMask) != 0) {
			
			Debug.Log ("FIRE " + WeaponMask + "|" + (this.WeaponMask | WeaponMask));
			Debug.Log ("Cooldown: " + cooldownTimer);
			// fire
			if(released && cooldownTimer <= 0){
				Debug.Log ("GO");
				GameObject obj = Instantiate (Bullet);
				Bullet bullet = obj.GetComponent<Bullet> ();
				bullet.faction = rootShip.faction;
				obj.transform.position = Muzzle.transform.position;
				obj.transform.rotation = Muzzle.transform.rotation;
				released = false;
				cooldownTimer = cooldown;
			}
		} else {
			// stop fire
			released = true;
			Debug.Log (WeaponMask + "|" + (this.WeaponMask | WeaponMask));
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotWeapon : Weapon {

	[Range(0,10)]
	public float cooldown;
	protected float cooldownTimer;

	[Range(0, 1000)]
	public int energyCost;

	public void FixedUpdate() {
		if (cooldownTimer > 0) {
			cooldownTimer -= Time.fixedDeltaTime;
		}
	}

}

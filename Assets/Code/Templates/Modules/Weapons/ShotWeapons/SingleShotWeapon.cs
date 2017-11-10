using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : ShotWeapon {
	
	public GameObject bulletPrefab;
	public GameObject Muzzle;

	private Bullet bulletComponent;

	public bool released = true;

	void Start(){
		annie = GetComponent<Animator> ();
		if (annie == null) {
			annie = gameObject.AddComponent<Animator> ();
		}
		bulletComponent = bulletPrefab.GetComponent<Bullet> ();
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
				if (rootShip.energyCur < energyCost) {
					return;
				}

				released = false;
				cooldownTimer = cooldown;
				annie.SetTrigger ("Fire");
				annie.SetBool ("Released", false);

				Bullet bullet = BulletPoolManager.instance.Get(bulletComponent.UNIQUE_NAME);
				if (bullet == null) {
					return;
				}

				// orient bullet
				bullet.faction = rootShip.faction;
				bullet.transform.position = Muzzle.transform.position;
				bullet.transform.rotation = Muzzle.transform.rotation;

				// activate bullet
				bullet.gameObject.SetActive (true);

				rootShip.energyCur -= energyCost;
			}
		} else {
			// stop fire
			annie.SetBool("Released", true);
			released = true;
		}
	}
}

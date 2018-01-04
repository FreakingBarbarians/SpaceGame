﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : ShotWeapon {
	public GameObject bulletPrefab;
	public GameObject Muzzle;
	private bool fire = false;

	private Bullet bulletComponent;

	protected override void Init ()
	{
		base.Init ();
		bulletComponent = bulletPrefab.GetComponent<Bullet> ();
		annie.speed = 1f / cooldown;
	}

	public new void FixedUpdate(){
		if (!operational) {
			return;
		}

		if (cooldownTimer > 0) {
			cooldownTimer -= Time.fixedDeltaTime;
		}

		if (fire) {
			while (cooldownTimer <= 0 && rootShip.EnergyCur >= energyCost) {
				cooldownTimer += cooldown;
				annie.SetTrigger ("Fire");
				Bullet bull = BulletPoolManager.instance.Get (bulletComponent.BASE_NAME);
				bull.faction = rootShip.faction;
				bull.transform.position = Muzzle.transform.position;
				bull.transform.rotation = Muzzle.transform.rotation;
				bull.damage = Damage;
				bull.velocity = Velocity;
				bull.gameObject.SetActive (true);
				rootShip.EnergyCur -= energyCost;

                Sound s = SoundPool.instance.Get(ShotSound);
                s.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
                s.gameObject.SetActive(true);
                s.Play();
            }

			if (rootShip.EnergyCur < energyCost) {
				cooldownTimer = 0;
			}

		}
	}

	public override void UpdateWeaponState (int WeaponMask)
	{
		fire = ((this.WeaponMask & WeaponMask) != 0);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : Damager {
	public FACTION faction;

	[Range(1,10)]
	public float velocity;
	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
		rb.gravityScale = 0;
		rb.mass = 1;
		rb.velocity = transform.up * velocity;
	}

	public void OnTriggerEnter2D(Collider2D col){
		Damageable damager = col.gameObject.GetComponent<Damageable> ();

		if (damager.faction == this.faction) {
			return;
		}

		if (damager != null) {
			damager.DoDamage (damage);
		}

		GameObject.Destroy (this.gameObject);
	}
}

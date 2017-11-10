using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : Damager, IPoolable {
	public string UNIQUE_NAME;

	public FACTION faction;

	[Range(1,10)]
	public float velocity;
	private Rigidbody2D rb;

	private bool initialized = false;

	public string GetId ()
	{
		return UNIQUE_NAME;	
	}

	void Start(){
		if (rb == null) {
			rb = GetComponent<Rigidbody2D> ();
		}
		rb.gravityScale = 0;
		rb.mass = 1;
	}

	void OnEnable(){
		if (!initialized) {
			Start ();
		}
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

		gameObject.SetActive (false);
		BulletPoolManager.instance.Free (UNIQUE_NAME, this);
	}
}

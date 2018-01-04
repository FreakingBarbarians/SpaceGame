using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : Damager, IPoolable {
	public string BASE_NAME;
	public string BASE_PATH;

	public FACTION faction;

	[Range(1,10)]
	public float velocity;

	[Range(1,60)]
	public float LifeTime;
	private float LifeTimer;

	private Rigidbody2D rb;

	private bool initialized = false;

	public string GetId ()
	{
		return BASE_NAME;	
	}

	public void FixedUpdate() {
		if (LifeTimer <= 0) {
			gameObject.SetActive (false);
			BulletPoolManager.instance.Free (BASE_NAME, this);
		}
		LifeTimer -= Time.deltaTime;
	}

	void Start(){
		if (rb == null) {
			rb = GetComponent<Rigidbody2D> ();
		}
		rb.gravityScale = 0;
		rb.mass = 1;
		LifeTimer = LifeTime;
	}

	void OnEnable(){
		if (!initialized) {
			Start ();
		}
		rb.velocity = transform.up * velocity;
		LifeTimer = LifeTime;
	}

	public void OnTriggerEnter2D(Collider2D col){
		Damageable damager = col.gameObject.GetComponent<Damageable> ();

		if (damager == null) {
			Debug.Log(col.gameObject.name);
		}

		if (damager.faction == this.faction) {
			return;
		}

		if (damager != null) {
			damager.DoDamage (damage);
			WidgetManager.instance.CreateFloatingNumber (Color.red, Color.clear, 1, 0.1f, damage.ToString ()).transform.position = transform.position;
            Sound s = SoundPool.instance.Get("BULLET_IMPACT");
            s.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
            s.gameObject.SetActive(true);
            s.Play();
		}

		gameObject.SetActive (false);
		BulletPoolManager.instance.Free (BASE_NAME, this);
	}
}

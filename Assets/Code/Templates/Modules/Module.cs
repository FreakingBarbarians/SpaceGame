using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Module : Damageable {
	public enum ModuleType {
		Standard,
		Weapon
	}

    public Port.PortType portType;
	public ModuleType moduleType;
    public bool operational = true;
	public bool adrift = false;
	protected Port root;
	protected Ship rootShip;

	protected Animator annie;

	void Start(){
		annie = GetComponent<Animator> ();
		if (annie == null) {
			annie = gameObject.AddComponent<Animator> ();
		}

	}

    public void Register (Port port) {
        this.root = port;
    }

	// needed?
    public void OnConnect() {
        // make CONNECTIONS
    }

	// probably not :/
    public void OnDisconnect() {
        // destroy CONNECTIONS
    }

	public void RegisterShip(Ship ship) {
		this.rootShip = ship;
	}

	public override void DoDamage (int amt)
	{
		annie.SetTrigger ("Damaged");
		this.curhp = Mathf.Max (curhp, 0);
		if (curhp == 0) {
			Die ();
		}
	}

	public override void DoHeal (int amt)
	{
		this.curhp += amt;
		if (operational == false) {
			// revive
		}
	}

	public override void Die()
	{
		annie.SetBool ("Die",true);
		operational = false;
		gameObject.layer = LayerMask.NameToLayer ("Debris");
		// other stuff
	}

	public void SetAdrift(){
		this.adrift = true;

		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D> ();

		Vector2 away = transform.position - rootShip.transform.position;
		rb.velocity = away.normalized * Random.value;
		gameObject.layer = LayerMask.NameToLayer ("Debris");
		rb.gravityScale = 0;
		// @TODO: pick up logic & set logic
	}

}

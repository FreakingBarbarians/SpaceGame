using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Set this script to execute before port
// I'm probably abusing partial here and not using S.R.P properly :(
// oh well.

public partial class Ship : Damageable {

    public float speedMax;
    public float speedCur;

	public int energyMax;
	public int energyCur;

	public int energyRegen;

	private Animator annie;

    public List<Port> ports = new List<Port>();
    public List<Port> mainPorts = new List<Port>();
	public List<Weapon> weapons = new List<Weapon>();

	private float timer = SpaceGameGlobal.TICK_RATE;

	private Vector2 MoveDirection;

    private void Start()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Ship");
		annie = GetComponent<Animator> ();
        // register all of our ports
        foreach (Port p in ports) {
            p.Register(this);
        }

        foreach (Port p in mainPorts) {
            p.Register(this);
        }
    }

	public void FixedUpdate(){
		timer -= Time.deltaTime;
		if (timer <= 0) {
			timer += SpaceGameGlobal.TICK_RATE;
			tick ();
		}
		transform.position += (Vector3) MoveDirection * Time.fixedDeltaTime * speedCur;
	}

	private void tick(){
		energyCur = Mathf.Min (energyMax, energyCur + energyRegen);
	}

	public void ChangeDirection(Vector2 dir){
		this.MoveDirection = dir;
	}

	public override void Die ()
	{
		this.enabled = false;
		foreach (Port p in ports) {
			p.Eject ();
		}
		foreach (Port p in mainPorts) {
			p.Eject ();
		}

		// some advanced code
		/*
		 * Goal: Each module will be set "drifting" and dissappear when leaving the camera bounds
		 * Modules can "Survive" the explosion where they can be picked up by the ship via contact.
		 * And used in the ship later on
		 */

	}	
}

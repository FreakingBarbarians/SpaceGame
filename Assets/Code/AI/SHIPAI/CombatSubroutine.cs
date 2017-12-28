using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
using QventSystem;

public class CombatSubroutine : StellarSubroutine {
	public List<Ship> targets = new List<Ship>();
	public float ScanRadius;
	public float ChaseRange;
	public float RefreshRate;
	private float RefreshRateTimer;
	private CircleCollider2D combatCollider;

	public void FixedUpdate(){
		if (RefreshRateTimer <= 0) {
			CullEnemies ();
			RefreshRateTimer += RefreshRate;
		}
		RefreshRateTimer -= Time.fixedDeltaTime;
	}

	protected override void init ()
	{
		base.init ();
		gameObject.layer = LayerMask.NameToLayer ("ShipOnly");
		// set up collider
		combatCollider = gameObject.AddComponent<CircleCollider2D> ();
		combatCollider.radius = ScanRadius;
		combatCollider.isTrigger = true;
		combatCollider.enabled = true;

	}

	private void FindEnemies(){
		RaycastHit2D[] hits = null;
		int count = combatCollider.Cast (new Vector2(1,1), hits);
		for (int i = 0; i < count; i++) {
			Ship ship = hits [i].collider.gameObject.GetComponent<Ship> ();
			Debug.Log (ship.name);
			if (ship.faction != cachedRoot.faction) {
				targets.Add (ship);
			}
		}
	}

	private void CullEnemies() {
		List<Ship> toRemove = new List<Ship> ();
		foreach (Ship ship in targets) {
			if (Vector2.Distance (ship.transform.position, transform.position) >= ChaseRange || ship.curhp <= 0) {
				toRemove.Add (ship);
			}
		}
		foreach (Ship ship in toRemove) {
			targets.Remove (ship);
		}
	}

	public override void HandleQvent (Qvent Qvent)
	{
		FindEnemies ();
		if (!Parent) {
			aiSystem.ActiveRoutine = this;
			Run ();
		}
	}

	void OnTriggerEnter2D (Collider2D coll){
		FindEnemies ();
		Ship ship;
		if ((ship = coll.gameObject.GetComponent<Ship>())) {
			if (ship.faction != cachedRoot.faction) {
				aiSystem.HandleQvent (new Qvent (QventType.SHIP_DETECTED));
				targets.Add (ship);
			}
		}
	}
}

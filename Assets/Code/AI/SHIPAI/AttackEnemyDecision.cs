using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
using QventSystem;

public class AttackEnemyDecision : StellarProcess {
	private CombatSubroutine cachedSubroutine;
	private bool bit = false;
	public float TargetDistance;

	protected override void onBegin ()
	{
		if (!(subRoutine is CombatSubroutine)) {
			Debug.LogError ("Combat Routine in a non combat subroutine");
			Destroy (this);
		}
		base.onBegin ();
		cachedSubroutine = (CombatSubroutine) subRoutine;
		cachedRoot.SetState (Ship.ShipState.COMBAT);
	}

	public override void Process ()
	{
		if (cachedSubroutine.targets.Count <= 0) {
			onFinish (StellarStatus.SUCCESS);
			return;
		}

		Vector2 dir = cachedSubroutine.targets [0].transform.position - transform.position;
		Vector2 holdPos = dir;

		holdPos.Normalize ();
		holdPos = (-holdPos * TargetDistance + (Vector2)cachedSubroutine.targets [0].transform.position) - (Vector2)transform.position;
		if (holdPos.magnitude <= 0.5f) {
			cachedRoot.Brake ();
		} else {
			cachedRoot.Thrust (holdPos);
		}
		cachedRoot.RotateTowards (dir);
		cachedRoot.PointWeaponsTowards (cachedSubroutine.targets[0].transform.position);

		if(bit) {
			cachedRoot.Fire (~0);
			bit = !bit;
		} else {
			cachedRoot.Fire(0);
			bit = !bit;
		}

		cachedRoot.SetState (Ship.ShipState.COMBAT);
		// @TODO: weapon ctonrol and fireing.
	}

	protected override void onFinish (StellarStatus status)
	{
		base.onFinish (status);
		cachedRoot.Fire (0);
	}

	public override void OnInterrupt ()
	{
		cachedRoot.Fire(0);
		base.OnInterrupt ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
using QventSystem;

public class AttackEnemyDecision : StellarProcess {
	private CombatSubroutine cachedSubroutine;
	private bool bit = false;
	protected override void onBegin ()
	{
		if (!(subRoutine is CombatSubroutine)) {
			Debug.LogError ("Combat Routine in a non combat subroutine");
			Destroy (this);
		}
		base.onBegin ();
		cachedSubroutine = (CombatSubroutine) subRoutine;
		cachedRoot.Fire (~0); // all
	}

	public override void Process ()
	{
		if (cachedSubroutine.targets.Count <= 0) {
			onFinish (StellarStatus.SUCCESS);
			return;
		}

		Vector2 dir = cachedSubroutine.targets [0].transform.position - transform.position;
		cachedRoot.Thrust (dir);
		cachedRoot.RotateTowards (dir);
		cachedRoot.PointWeaponsTowards (cachedSubroutine.targets[0].transform.position);

		if(bit) {
			cachedRoot.Fire (~0);
			bit = !bit;
		} else {
			cachedRoot.Fire(0);
			bit = !bit;
		}

		// @TODO: weapon ctonrol and fireing.
	}

	protected override void onFinish (StellarStatus status)
	{
		cachedRoot.Fire (0);
		base.onFinish (status);
	}

	public override void OnInterrupt ()
	{
		cachedRoot.Fire(0);
		base.OnInterrupt ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
using QventSystem;

public class AttackEnemyDecision : StellarProcess {
	private CombatSubroutine cachedSubroutine;
	protected override void onBegin ()
	{
		if (!(subRoutine is CombatSubroutine)) {
			Debug.LogError ("Combat Routine in a non combat subroutine");
			Destroy (this);
		}
		base.onBegin ();
		cachedSubroutine = (CombatSubroutine) subRoutine;
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
		// @TODO: weapon ctonrol and fireing.

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
public class Brake : StellarProcess {
	public float TimeLimit = 3f;
	private float TimeLimitTimer = 0;

	protected override void onBegin ()
	{
		base.onBegin ();
		TimeLimitTimer = 0;
	}

	public override void Process ()
	{
		if (TimeLimitTimer >= TimeLimit) {
			onFinish (StellarStatus.FAIL);
			return;
		}

		if (cachedRoot.DeltaPosition.magnitude == 0 && cachedRoot.DeltaRotation == 0) {
			onFinish (StellarStatus.SUCCESS);
			return;
		}

		TimeLimitTimer += Time.fixedDeltaTime;

		cachedRoot.Brake ();
	}

	public override void OnInterrupt ()
	{
		base.OnInterrupt ();
	}
}

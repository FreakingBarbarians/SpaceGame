using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;

public class FollowMouseDecision : StellarProcess  {

	public float Range;

	public override void Process ()
	{
		Vector2 dir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - cachedRoot.transform.position;

		if (dir.magnitude >= Range) {
			onFinish(StellarStatus.FAIL);
			return;
		}

		cachedRoot.Thrust (dir);
		cachedRoot.RotateTowards (dir);
	}
}

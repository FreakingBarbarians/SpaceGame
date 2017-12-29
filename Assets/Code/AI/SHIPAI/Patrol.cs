using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;

public class Patrol : StellarProcess {
	public List<Vector3> PatrolSequence;
	public float PatrolRadius;
	public int Points;
	private int index = 0;

	protected override void onBegin ()
	{
	
		base.onBegin ();
		PatrolSequence.Clear ();
		index = 0;
		for (int i = 0; i < Points; i++) {
			Vector2 PatrolPoint = Random.insideUnitCircle * PatrolRadius;
			PatrolPoint += (Vector2) cachedRoot.transform.position;
			PatrolSequence.Add (PatrolPoint);
		}
	
	}

	public override void Process ()
	{
		if (index >= PatrolSequence.Count) {
			onFinish (StellarStatus.SUCCESS);
			return;
		}

		Vector2 delta = PatrolSequence [index] - cachedRoot.transform.position;
		cachedRoot.Thrust (delta);
		cachedRoot.RotateTowards (delta);

		if (delta.magnitude < 0.5f) {
			index++;
		}

	}
}

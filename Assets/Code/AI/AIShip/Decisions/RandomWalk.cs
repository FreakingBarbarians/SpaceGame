using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomWalk : Decision {

	public float VectorChangeTime;
	private float VectorChangeTimer = 0;
	private Vector2 dir;

	public override NodeStatus Run(float deltaTime) {
		if  (VectorChangeTimer <= 0) {
			dir = new Vector2 (UnityEngine.Random.Range (-1f, 1f), UnityEngine.Random.Range (-1f, 1f));
			VectorChangeTimer += VectorChangeTime;
		}
		VectorChangeTimer -= Time.fixedDeltaTime;
		target.Thrust (dir);
		// @TODO: Check for engine failure maybe...
		status = NodeStatus.RUNNING;
		return NodeStatus.RUNNING;
	}

	public override NodeStatus EventRun(float deltaTime, Type type, object o) {
		Debug.LogWarning ("Unimplemented EventRun in Abstract \"DECISION\" Class");
		return NodeStatus.FAIL;
	}
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//
//public class RandomWalk : Decision<AIShip> {
//
//	public float VectorChangeTime;
//	private float VectorChangeTimer = 0;
//	private Vector2 dir;
//
//	public override NodeStatus Run(AIShip source, float deltaTime) {
//		if  (VectorChangeTimer <= 0) {
//			dir = new Vector2 (UnityEngine.Random.Range (-1, 1), UnityEngine.Random.Range (-1, 1));
//			VectorChangeTimer += VectorChangeTimer;
//			VectorChangeTimer -= Time.fixedDeltaTime;
//		}
//		source.target.Thrust (dir);
//		return NodeStatus.RUNNING;
//	}
//
//	public override NodeStatus EventRun(AIShip Source, float deltaTime, Type type, object o) {
//		Debug.LogWarning ("Unimplemented EventRun in Abstract \"DECISION\" Class");
//		return NodeStatus.FAIL;
//	}
//}

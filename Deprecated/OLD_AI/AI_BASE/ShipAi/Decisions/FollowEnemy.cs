//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using QventSystem;
//public class FollowEnemy : Decision<AIShip> {
//	
//	private Ship enemy;
//
//	[Range(1,100)]
//	public float FollowRange;
//
//	public override NodeStatus Run(AIShip source, float deltaTime) {
//		// stops following if the enemy is null, the enemies' faction is the source faction
//		if (!enemy || enemy.faction == source.Faction || Vector2.Distance(enemy.transform.position, transform.position) > FollowRange) {
//			return NodeStatus.FAIL;
//		} else {
//			// @TODO: Better move logic
//			source.target.Thrust((enemy.transform.position - transform.position));
//			source.target.RotateTowards (enemy.transform.position);
//
//			return NodeStatus.RUNNING;
//		}
//	}
//
//	public override NodeStatus EventRun(AIShip Source, float deltaTime, Type type, object o) {
//		// @TODO: Can put a switch here to demult between types
//		// really eventrun is only one that needs to notify
//		NodeStatus retval;
//		Qvent qvent;
//		Ship enemy = (Ship)o;
//		List<object> data = new List<object> ();
//		data.Add (this);
//		if (!enemy || enemy.faction == Source.Faction) {
//			data.Add (NodeStatus.FAIL);
//			retval = NodeStatus.FAIL;
//		} else {
//			data.Add (NodeStatus.RUNNING);
//			this.enemy = enemy;
//			retval = NodeStatus.RUNNING;
//		}
//		qvent = new Qvent(QventType.DECISION_EVENT, typeof(Decision<AIShip>), data); 
//		foreach (QventHandler handler in Listeners) {
//			handler.HandleQvent (qvent);
//		}
//		return retval;
//	}
//}

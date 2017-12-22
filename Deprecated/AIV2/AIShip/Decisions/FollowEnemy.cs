using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
public class FollowEnemy : Decision {
	
	private Ship enemy;

	[Range(1,100)]
	public float FollowRange;

	public override NodeStatus Run(float deltaTime) {
		// stops following if the enemy is null, the enemies' faction is the source faction

		if (!enemy || enemy.faction == target.faction || Vector2.Distance(enemy.transform.position, transform.position) > FollowRange) {
			status = NodeStatus.IDLE;
			return NodeStatus.FAIL;
		} else {
			// @TODO: Better move logic
			target.Thrust((enemy.transform.position - transform.position));
			target.RotateTowards (enemy.transform.position- transform.position);
			status = NodeStatus.RUNNING;
			return NodeStatus.RUNNING;
		}
	}

	public override NodeStatus EventRun(float deltaTime, Type type, object o) {
		// @TODO: Can put a switch here to demult between types
		// really eventrun is only one that needs to notify
		NodeStatus retval;
		Qvent qvent;
		Ship enemy = (Ship)o;
		List<object> data = new List<object> ();

		data.Add (this);

		if (!enemy || enemy.faction == target.faction) {
			data.Add (NodeStatus.FAIL);
			status = NodeStatus.IDLE;
			retval = NodeStatus.FAIL;
		} else {
			data.Add (NodeStatus.RUNNING);
			this.enemy = enemy;
			status = NodeStatus.RUNNING;
			retval = NodeStatus.RUNNING;
		}

		qvent = new Qvent(QventType.DECISION_EVENT, typeof(Decision), data); 
		foreach (QventHandler handler in Listeners) {
			handler.HandleQvent (qvent);
		}

		return retval;
	}
}

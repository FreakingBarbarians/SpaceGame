using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
using System;
public class FollowEnemy : Decision<AIShip> {

	private Ship enemy;

	public override NodeStatus Run(AIShip source, float deltaTime) {
		if (!enemy || enemy.faction == source.Faction) {
			return NodeStatus.FAIL;
		} else {
			// @TODO: Move logic...
			return NodeStatus.RUNNING;
		}
	}

	public override NodeStatus EventRun(AIShip Source, float deltaTime, Type type, object o) {
		// @TODO: Can put a switch here to demult between types
		Ship enemy = (Ship)o;
		if (!enemy || enemy.faction == Source.Faction) {
			return NodeStatus.FAIL;
		} else {
			this.enemy = enemy;
			return NodeStatus.RUNNING;
		}
	}
}

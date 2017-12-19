using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
using System;
public class BehaviorTreeAddShortcut : MonoBehaviour {

	private bool run = false;
	public AIShip Target;
	public FollowEnemy NodeToAdd;
	// Update is called once per frame
	void Update () {
		if (!run) {
			BehaviorTree<AIShip> behave = new BehaviorTree<AIShip>(Target);
			behave.AddShortcut (QventType.SHIP_DETECTED, NodeToAdd);
			Target.SetBehaviorTree(behave);
		}
		run = true;
	}
}

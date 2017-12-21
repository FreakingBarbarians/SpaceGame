using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
using System;
public class BehaviorTreeAddShortcut : MonoBehaviour {
	public AIBase BehaviorTree;
	public QventType QventType;

	void Start(){
		BehaviorTree.AddTrigger (QventType, GetComponent<Decision>());
	}

}

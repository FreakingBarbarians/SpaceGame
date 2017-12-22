using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class XorGroup : Group {
	void Start(){
		init ();
	}

	public override void HandleQvent(Qvent e){
		Debug.Log ("RECIEVED EVENT IN XOR GROUP");
		if (e.QventType == QventType.DECISION_EVENT) {
			List<object> list = (List<object>) e.Payload;
			Decision decision = (Decision) list [0];
			NodeStatus status = (NodeStatus) list [1];
			foreach(Decision member in GroupMembers){
				if (decision != member) {
					source.QueueRemove (member);
				}
			}
		}
	}
}
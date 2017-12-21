using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

//public class AIShipXOR : Group<AIShip>{
//	void Start(){
//		init ();
//	}
//
//	public override void HandleQvent(Qvent e){
//		if (e.QventType == QventType.DECISION_EVENT) {
//			List<object> list = (List<object>) e.Payload;
//			Decision<AIShip> decision = (Decision<AIShip>) list [0];
//			NodeStatus status = (NodeStatus) list [1];
//			foreach(Decision<AIShip> member in GroupMembers){
//				if (decision != member) {
//					source.QueueRemove (member);
//				}
//			}
//		}
//	}
//}
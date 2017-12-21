//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using QventSystem;
//
//// @TODO: Better name
//public enum NodeStatus
//{
//	FAIL,
//	RUNNING,
//	SUCCESS
//}
//
//public abstract class Decision<T> : Node<T>, IEventEmitter {
//
//	protected List<QventHandler> Listeners = new List<QventHandler>();
//
//	public virtual NodeStatus Run(T source, float deltaTime) {
//		Debug.LogWarning ("Unimplemented Run in Abstract \"DECISION\" Class");
//		return NodeStatus.FAIL;
//	}
//
//	public virtual NodeStatus EventRun(T Source, float deltaTime, Type type, object o) {
//		Debug.LogWarning ("Unimplemented EventRun in Abstract \"DECISION\" Class");
//		return NodeStatus.FAIL;
//	}
//
//	public void RegisterListener(QventHandler handler){
//		if (!Listeners.Contains (handler)) {
//			Listeners.Add (handler);
//		}
//	}
//
//	public void UnregisterListener(QventHandler handler){
//		Listeners.Remove (handler);
//	}
//
//	public override List<Decision<T>> Continue ()
//	{
//		List<Decision<T>> list = new List<Decision<T>> ();
//		list.Add (this);
//		return list;
//	}
//}

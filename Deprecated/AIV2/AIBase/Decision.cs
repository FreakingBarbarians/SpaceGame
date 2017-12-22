using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public abstract class Decision : Node, IQventEmitter {
	protected List<QventHandler> Listeners = new List<QventHandler>();

	public virtual NodeStatus Run(float deltaTime) {
		Debug.LogWarning ("Unimplemented Run in Abstract \"DECISION\" Class");
		return NodeStatus.FAIL;
	}

	public virtual NodeStatus EventRun(float deltaTime, Type type, object o) {
		Debug.LogWarning ("Unimplemented EventRun in Abstract \"DECISION\" Class");
		return NodeStatus.FAIL;
	}

	public void RegisterListener(QventHandler handler){
		if (!Listeners.Contains (handler)) {
			Listeners.Add (handler);
		}
	}

	public void UnregisterListener(QventHandler handler){
		Listeners.Remove (handler);
	}

	public override List<Decision> Continue ()
	{
		List<Decision> list = new List<Decision> ();
		list.Add (this);
		return list;
	}

}

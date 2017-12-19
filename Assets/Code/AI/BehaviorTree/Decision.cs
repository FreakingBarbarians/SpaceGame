using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @TODO: Better name
public enum NodeStatus
{
	FAIL,
	RUNNING,
	SUCCESS
}

public abstract class Decision<T> : Node<T> {
	public virtual NodeStatus Run(T source, float deltaTime) {
		Debug.LogWarning ("Unimplemented Run in Abstract \"DECISION\" Class");
		return NodeStatus.FAIL;
	}

	public virtual NodeStatus EventRun(T Source, float deltaTime, Type type, object o) {
		Debug.LogWarning ("Unimplemented EventRun in Abstract \"DECISION\" Class");
		return NodeStatus.FAIL;
	}
}

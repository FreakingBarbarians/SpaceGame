using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecisionResult
{
	FAIL,
	RUNNING,
	SUCCESS
}

public abstract class Decision<T> : Node {
	public virtual DecisionResult Run(T source, float deltaTime){
		Debug.LogWarning ("Unimplemented Run in Abstract \"DECISION\" Class");
	}
}

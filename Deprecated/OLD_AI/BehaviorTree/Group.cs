using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
//// @TODO: Think about merging decision & node?
//public abstract class Group<T> : MonoBehaviour, QventHandler {
//	protected T source;
//	public List<Component> GroupMembers;
//
//	protected virtual void init(){
//		foreach (Component member in  GroupMembers) {
//			((Decision<T>)member).RegisterListener (this);
//		}
//	}
//
//	public virtual void HandleQvent(Qvent Qvent){
//		Debug.LogWarning ("Unimplemented HandleQvent called in Group<T>");
//	}
//
//	public void Register(T source){
//		this.source = source;
//	}
//}

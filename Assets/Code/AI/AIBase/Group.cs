using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
// @NEW_AIBaseODO: NEW_AIBasehink about merging decision & node?
public abstract class Group : MonoBehaviour, QventHandler {
	protected AIBase source;
	public List<Decision> GroupMembers;

	protected virtual void init(){
		foreach (Decision member in  GroupMembers) {
			member.RegisterListener (this);
		}
	}

	public virtual void HandleQvent(Qvent Qvent){
		Debug.LogWarning ("Unimplemented HandleQvent called in Group<NEW_AIBase>");
	}

	public void Register(AIBase source){
		this.source = source;
	}
}

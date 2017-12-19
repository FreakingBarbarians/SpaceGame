using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public abstract class AIBase<T> : IUnityXmlSerializable, QventHandler {

	// @TODO: Write serialization for Dictionaries in XmlUtil Class.
	protected Dictionary<string, float> floatAttributes;
	protected Dictionary<string, bool> boolAttributes;

	protected BehaviorTree<T> Tree;

	public void FixedUpdate(){
		Tree.Process ();
	}

	public void HandleQvent(Qvent evt){
		// evt handling
		switch (evt.QventType) {
		case QventType.SHIP_DETECTED:
			Debug.Log ("Ship Detected");
			break;
		}

		Tree.HandleQvent(evt);
	}

	public void SetBehaviorTree(BehaviorTree<T> Tree){
		this.Tree = Tree;
		Debug.Log ("set");
	}

	public float GetFloatAttribute(string FloatName){
		if(floatAttributes.ContainsKey(FloatName)) {
			return floatAttributes[FloatName];
		} else {
			Debug.LogWarning("No value : " + FloatName + "in AiBase of : " + gameObject.name);
			return float.MinValue;
		}
	}

	public void SetFloatAttribute(string FloatName, float Value){
		floatAttributes [FloatName] = Value;
	}

	public bool GetBoolAttribute(string BoolName){
		if(floatAttributes.ContainsKey(BoolName)) {
			return boolAttributes[BoolName];
		} else {
			Debug.LogWarning("No value : " + BoolName + "in AiBase of : " + gameObject.name);
			return false;
		}
	}

	public void SetBoolAttribute(string BoolName, bool Value){
		boolAttributes [BoolName] = Value;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

// maybe not serializable and just initialized as a side-effect
public abstract class EventEmitter : IUnityXmlSerializable {
	public List<QventHandler> Listeners = new List<QventHandler> ();

	public void RegisterListener(QventHandler Listener){
		Listeners.Add (Listener);
	}

	public void UnregisterListener(QventHandler Listener){
		Listeners.Remove (Listener);
	}

	public virtual void DispatchEvent(){

	}
}

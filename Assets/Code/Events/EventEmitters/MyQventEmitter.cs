using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class MyQventEmitter : MonoBehaviour, IQventEmitter {
	public List<QventHandler> Listeners = new List<QventHandler>();

	public virtual void DispatchEvent(Qvent q) {
		foreach(QventHandler handler in Listeners){
			handler.HandleQvent(q);
		}
	}

	public void RegisterListener (QventHandler Listener){
		Listeners.Add (Listener);
	}

	public void UnregisterListener (QventHandler Listener) {
		Listeners.Remove (Listener);
	}
}
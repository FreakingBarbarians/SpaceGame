using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class OnTriggerEnter2DEvent : MonoBehaviour, IEventEmitter {
	Collider2D colly;
	public List<QventHandler> Listeners = new List<QventHandler>();

	void Start() {
		colly = GetComponent<Collider2D> ();
		colly.isTrigger = true;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Qvent evt = new Qvent (QventType.SHIP_DETECTED, typeof(Ship), coll.gameObject.GetComponent<Ship>());
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (evt);
		}
	}

	public void RegisterListener(QventHandler handler){
		if (!Listeners.Contains (handler)) {
			Listeners.Add (handler);
		}
	}

	public void UnregisterListener(QventHandler handler){
		Listeners.Remove (handler);
	}
}

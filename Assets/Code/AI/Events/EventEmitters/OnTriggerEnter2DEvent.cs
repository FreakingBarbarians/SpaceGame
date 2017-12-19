using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class OnTriggerEnter2DEvent : EventEmitter {
	Collider2D colly;

	void Start() {
		colly = GetComponent<Collider2D> ();
		colly.isTrigger = true;
	}

	void OnTriggerEnter2D(Collider2D coll){
		Qvent evt = new Qvent (QventType.SHIP_DETECTED, typeof(Ship), coll.gameObject.GetComponent<Ship>());
		foreach (QventHandler handy in Listeners) {
			handy.HandleQvent (evt);
		}
	}
}

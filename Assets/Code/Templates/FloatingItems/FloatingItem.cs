using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class FloatingItem : MyPrefab, IQventEmitter {

	protected SpriteRenderer rendy;
	public SpriteRenderer itemRenderer;

	void Start() {
		rendy = GetComponent<SpriteRenderer> ();
	}

	public virtual void OnPickup(){
		Debug.Log ("Item get!");
	}

	public List<QventHandler> Listeners = new List<QventHandler> ();

	public void RegisterListener (QventHandler Listener) {
		if (!Listeners.Contains (Listener)) {
			Listeners.Add (Listener);
		}
	}

	public void UnregisterListener (QventHandler Listener) {
		Listeners.Remove (Listener);
	}

	public void OnTriggerEnter2D(Collider2D coll){
		OnPickup ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 100 by 100 units. This is good
public class Sector : IUnityXmlSerializable {

	public List<GameObject> Objects = new List<GameObject>();
	public bool Loaded = false;
	public Vector2Int index;

	public void Unload() {
		foreach (GameObject obj in Objects) {
			obj.SetActive (false);
		}
	}

	public void Load() {
		foreach (GameObject obj in Objects) {
			obj.SetActive (true);
		}
	}

	public void Check() {
		List<GameObject> toRemove = new List<GameObject> ();

		foreach (GameObject o in Objects) {
			if (!GalaxyManager.instance.WorldToSectorPoint (o.transform.position).Equals (index)) {
				GameObject obj = o;
				toRemove.Add (obj);
				GalaxyManager.instance.AddToSector (obj);
			}
		}

		foreach (GameObject o in toRemove) {
			Objects.Remove (o);	
		}

	}
}

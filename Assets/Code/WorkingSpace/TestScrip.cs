using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScrip : MonoBehaviour {

	public GameObject prefab;
	private bool run = false;


	private void Update(){

		if (!run) {
			run = true;
			GalaxyManager.SpawnWorldObject (prefab, Vector3.zero);
		}
	}
}

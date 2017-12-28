using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo1TestScript : MonoBehaviour {
	public string resourcePath;
	public Ship s;

	void Update(){
		if(Input.GetKeyDown(KeyCode.L)) {
			s.BeginRepair ();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo1TestScript : MonoBehaviour {
	public string resourcePath;
	public FloatingSchematic floaty;

	void Update(){
		if(Input.GetKeyDown(KeyCode.L)) {
			floaty.SetItem(resourcePath);	
		}
	}
}

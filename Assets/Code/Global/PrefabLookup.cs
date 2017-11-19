using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLookup : MonoBehaviour {
	private Dictionary<string, string> map = new Dictionary<string,string>();


	public PrefabLookup instance;

	void Start(){
		if (instance != null) {
			Destroy (this);
			return;
		}
	}
}

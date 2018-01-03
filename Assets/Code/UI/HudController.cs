using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {
	public static HudController instance;

	void Start(){
		if (instance) {
			Debug.LogError ("More than one hud controller");
			return;
		}
		instance = this;
	}

	public void Enable(){
		gameObject.SetActive (true);
	}

	public void Disable(){
		gameObject.SetActive (false);
	}

}

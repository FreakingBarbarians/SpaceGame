using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : MonoBehaviour {
	public Material StarField;
	public static AtmosphereManager instance;
	public float parallaxVal;
	public float StarLayers;


	void Start() {
		if (instance) {
			Debug.LogWarning ("Two atmosphere Manager instances");
			return;
		}
		instance = this;
	}

	void Update() {
		StarField.SetTextureOffset ("_MainTex", Camera.main.transform.position/parallaxVal);
	}

}

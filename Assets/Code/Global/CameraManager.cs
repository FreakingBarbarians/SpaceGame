using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	private GameObject toFollow;
	private Vector3 offset;
	public static CameraManager instance;

	public float LerpFactor = 0.05f;
	public float Tolerance = 0.1f;

	private bool LerpToTarget = false;
	private bool IgnoreTolerance = false;

	private bool inRange = true;

	public int maxSize = 50;
	public int minSize = 3;

	// Use this for initialization
	void Start () {
		if (instance) {
			Debug.LogWarning ("More than one camera manager");
		}
		instance = this;
	}
		
	void Update () {
		if (!toFollow) {
			return;
		}

		if (!IgnoreTolerance) {
			if (Vector3.Distance (Camera.main.transform.position, toFollow.transform.position + offset) >= Tolerance) {
				LerpToTarget = true;
				inRange = false;
			} else {
				inRange = true;
			}
		}

		if(LerpToTarget){
			Vector3 newpos = Vector3.Lerp (Camera.main.transform.position, toFollow.transform.position + offset, Time.deltaTime * LerpFactor);
			Camera.main.transform.position = newpos;
			LerpToTarget = false;
		}

		if (inRange) {
			Camera.main.transform.position = toFollow.transform.position + offset;
		}
	}

	public void SetOffset(Vector3 offset){
		this.offset = offset;
	}

	public void AddOffset(Vector3 offset) {
		this.offset += offset;
	}

	public void SetToFollow(GameObject obj){
		toFollow = obj;
	}

	public GameObject GetToFollow(){
		return toFollow;
	}

	public void IncreaseSize() {
		if (Camera.main.orthographicSize >= maxSize) {
			return;
		} else {
			Camera.main.orthographicSize += 1;
		}
	}

	public void DecreaseSize() {
		if (Camera.main.orthographicSize <= minSize) {
			return;
		} else {
			Camera.main.orthographicSize -= 1;
		}
	}
}

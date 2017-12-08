using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {
	public static CursorManager instance;
	public Sprite CursorTex;
	public Image CursorImage;
	public float CURSOR_BASE_SCALE;

	// Use this for initialization
	void Start () {
		if (instance) {
			Debug.LogWarning ("Multiple Cursor Manager Instances");
			return;
		}

		instance = this;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
		CURSOR_BASE_SCALE = Screen.width / 64;
		transform.localScale = new Vector3 (CURSOR_BASE_SCALE, CURSOR_BASE_SCALE, 1);
		CursorImage.sprite = CursorTex;
	}

	void FixedUpdate () {
		transform.position = Input.mousePosition;	
	}
}

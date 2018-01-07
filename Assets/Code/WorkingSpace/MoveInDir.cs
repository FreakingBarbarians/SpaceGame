using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDir : MonoBehaviour {

    public Vector2 Direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += (Vector3) Direction * Time.deltaTime;
	}
}

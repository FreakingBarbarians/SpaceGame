using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class ModuleAnimator : MonoBehaviour {
	// private SpriteRenderer rendy;
	private Animator annie;

	public void Start(){
		// rendy = GetComponent<SpriteRenderer> ();
		annie = GetComponent<Animator> ();
	}

}

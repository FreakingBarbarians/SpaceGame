using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class MouseEnterRange : MyQventEmitter {
	private bool InRange = false;
	public float range;

	public QventType QventType;

	public void Update() {
		Vector2 dir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		if (dir.magnitude <= range) {
			if (InRange) {
				return;
			} else {
				InRange = true;
				Qvent q = new Qvent (QventType);
				DispatchEvent (q);
			}
		} else {
			InRange = false;
		}
	}
}

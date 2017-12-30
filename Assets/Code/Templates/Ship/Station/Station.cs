using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Ship {
	
	void Start() {

		gameObject.layer = LayerMask.NameToLayer ("Ship");

		annie = GetComponent<Animator> ();
		// register all of our ports
		foreach (Port p in ports) {
			p.Register(this);
			Module m;
			if((m = p.GetModule())){
				m.RegisterListener(this);
			}
		}

		foreach (Port p in mainPorts) {
			p.Register(this);
			Module m;
			if((m = p.GetModule())){
				m.RegisterListener(this);
			}
		}
		AddHealthBar ();
	}

}

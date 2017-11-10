using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickingUI : MonoBehaviour {
	private float timer = SpaceGameGlobal.TICK_RATE;

	void LateUpdate(){
		Refresh ();
	}

	public virtual void Refresh(){
		
	}
}

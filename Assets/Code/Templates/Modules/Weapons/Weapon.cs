using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Module {
    // Look at this coolness!

    public enum WeaponClass {

    }

    public enum WeaponType {

    }
    
    
	protected int WeaponMask = 1;

	public virtual void UpdateWeaponState(int WeaponMask){
		if ((this.WeaponMask & WeaponMask) != 0) {
			// fire
			Debug.Log("FIRE");
		} else {
			// stop fire
			Debug.Log("STOP FIRE");
		}
	}
}

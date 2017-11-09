using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModule : Module {

	public override void DoDamage (int amt)
	{
		annie.SetTrigger ("Damaged");

		curhp -= amt;
		if (curhp <= 0) {
			curhp = 0;
			Die ();
		}

		if (rootShip != null) {
			rootShip.DoDamage (amt);
		}

	}

	// overridden from module
    public override void Die()
    {
		annie.SetBool ("Die",true);
		operational = false;
		gameObject.layer = LayerMask.NameToLayer ("Debris");
        // other stuff
    }
}

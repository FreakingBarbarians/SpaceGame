using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModule : Module {

	public override void DoDamage (int amt)
	{
		if (rootShip != null) {
			rootShip.DoDamage (amt);
		}
	}

	// overridden from module
    public override void Die()
    {
        operational = false;
        // other stuff
    }
}

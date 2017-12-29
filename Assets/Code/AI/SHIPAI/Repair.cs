using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
public class Repair: StellarProcess {

	protected override void onBegin ()
	{
		base.onBegin ();
		cachedRoot.BeginRepair ();
	}

	public override void Process ()
	{

		if (cachedRoot.State != Ship.ShipState.REPAIR) {
			onFinish (StellarStatus.FAIL);
			return;
		}

		bool Continue = false;
		if (cachedRoot.curhp < cachedRoot.maxhp) {
			Continue = true;
		}

		foreach(Port p in cachedRoot.ports) {
			Module m;
			if ((m = p.GetModule ())) {
				if (m.curhp < m.maxhp) {
					Continue = true;
				}
			}
		}

		foreach (Port p in cachedRoot.mainPorts) {
			Module m;
			if ((m = p.GetModule ())) {
				if (m.curhp < m.maxhp) {
					Continue = true;
				}
			}
		}

		if(!Continue){
			onFinish (StellarStatus.SUCCESS);
			return;
		}
	}

	protected override void onFinish (StellarStatus status)
	{
		cachedRoot.StopRepair ();
		base.onFinish (status);
	}

	public override void OnInterrupt ()
	{
		cachedRoot.StopRepair ();
		base.OnInterrupt ();
	}

}

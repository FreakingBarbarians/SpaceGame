using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
public class RunSubroutine : StellarNode {
	public StellarSubroutine routine;

	protected override void onBegin ()
	{
		base.onBegin ();
		routine.SetParent (this);
		routine.Run ();
	}

	protected override void onFinish (StellarStatus finstatus)
	{
		base.onFinish (finstatus);
		routine.SetParent (null);
	}

	public override void OnInterrupt ()
	{
		
		status = StellarStatus.IDLE;
		base.OnInterrupt ();
	}
}

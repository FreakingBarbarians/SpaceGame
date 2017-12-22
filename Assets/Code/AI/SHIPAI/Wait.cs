using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarAI;
public class Wait : StellarProcess {

	protected override void onBegin ()
	{
		base.onBegin ();
		cachedRoot.gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
	}

	public override void Process ()
	{
		cachedRoot.Brake ();
	}

	public override void OnInterrupt ()
	{
		status = StellarStatus.IDLE;
		cachedRoot.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
	}
}

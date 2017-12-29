using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StellarAI{
public class StellarSequence : StellarNode {
	protected int index = 0;

	protected override void onBegin ()
	{
		base.onBegin ();
		index = 0;
		if (index >= Children.Count) {
			onFinish (StellarStatus.SUCCESS);
		} else {
			Children [index].Run ();
			index++;
		}
	}

	public override void ChildFinished (StellarStatus finstatus)
	{
		switch (finstatus) {
			case StellarStatus.FAIL:
			onFinish (StellarStatus.FAIL);
			break;
		case StellarStatus.SUCCESS:
			if (index < Children.Count) {
				Children [index].Run ();
				index++;
			} else {
				onFinish (StellarStatus.SUCCESS);
			}
			break;
		}
	}
}
}
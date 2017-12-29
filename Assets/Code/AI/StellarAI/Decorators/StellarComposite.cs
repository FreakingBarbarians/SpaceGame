using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarAI {
	public class StellarComposite : StellarNode {
		protected int index = 0;

		protected override void onBegin ()
		{
			base.onBegin ();
			index = 0;
			if (index >= Children.Count) {
				onFinish (StellarStatus.FAIL);
			} else {
				Children [index].Run ();
				index++;
			}
		}

		public override void ChildFinished (StellarStatus finstatus)
		{
			if (finstatus == StellarStatus.FAIL) {
				if (index < Children.Count) {
					Debug.Log (index + "|" + Children.Count);
					Children [index].Run();
					index++;
				} else {
					onFinish (StellarStatus.FAIL);
				}
			} else if (finstatus == StellarStatus.SUCCESS) {
				index = 0;
				onFinish (StellarStatus.SUCCESS);
			}
		}
	}
}
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

		public override void ChildFinished (StellarStatus status)
		{
			if (status == StellarStatus.FAIL) {
				if (index <= Children.Count) {
					Children [index].Run();
					index++;
				} else {
					onFinish (StellarStatus.FAIL);
				}
			} else if (status == StellarStatus.SUCCESS) {
				index = 0;
				onFinish (StellarStatus.SUCCESS);
			}
		}
	}
}
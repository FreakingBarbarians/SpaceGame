using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

namespace StellarAI{
	public class StellarSubroutine :  StellarNode, QventHandler {
		// Some sort of advanced function here :/
		public virtual float CalculateUrgency() { return -1f;}

		protected override void onFinish (StellarStatus status)
		{
			if (Parent) {
				Parent.ChildFinished (StellarStatus.SUCCESS);
			} else {
				aiSystem.GoIdle ();
			}
		}

		public virtual void HandleQvent(Qvent Qvent){
			if (!Parent) {
				// @TODO: Tournament Logic To See If The SubRoutine Overrides Current SubRoutine
				aiSystem.ActiveRoutine = this;
			}
			// ... 
		}
	}
}
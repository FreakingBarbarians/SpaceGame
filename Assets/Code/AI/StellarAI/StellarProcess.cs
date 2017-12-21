using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

namespace StellarAI{
	public abstract class StellarProcess : StellarNode {

		public virtual void Process() {
			
		}

		public virtual void Run() {
			if (status != StellarStatus.RUNNING) {
				onBegin ();
			}
		}	

		protected override void onBegin () {
			base.onBegin ();
			aiSystem.ActiveProcess = this;
		}
			
		protected override void onFinish (StellarStatus status) {
			// call this before, since active process may be set to something else.
			aiSystem.ActiveProcess = null;
			base.onFinish (status);
		}
	}
}

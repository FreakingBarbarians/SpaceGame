using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

namespace StellarAI{
	public class StellarSubroutine :  StellarNode, QventHandler {
		// Some sort of advanced function here :/
		public virtual float CalculateUrgency() { return -1f;}
		public QventType Trigger;
		protected override void init ()
		{
			base.init ();
		}

		protected override void onFinish (StellarStatus finstatus)
		{
			
			Debug.Log (name + " Finished");
			status = StellarStatus.IDLE;
			if (Parent) {
				Parent.ChildFinished (finstatus);
			} else {
				aiSystem.GoIdle ();
			}

		}

		public override void ChildFinished(StellarStatus finstatus) {
			onFinish (finstatus);
		}

		public virtual void HandleQvent(Qvent Qvent){
			if (!Parent) {
				// @TODO: Tournament Logic To See If The SubRoutine Overrides Current SubRoutine
				aiSystem.ActiveRoutine = this;
				Run ();
			}
			// ... 
		}

		protected override void onBegin ()
		{
			base.onBegin ();
			if (Children.Count >= 1) {
				Children [0].Run ();
			}
		}

		public void SetRoot(StellarSystem system) {
			
			if (!setup) {
				init ();
			}

			aiSystem = system;
			cachedRoot = system.Root;
			foreach (StellarNode child in Children) {
				child.Register (this);
			} 
		}
	}
}
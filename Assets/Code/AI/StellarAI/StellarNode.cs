using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

namespace StellarAI {
	
	public enum StellarStatus {
		RUNNING,
		IDLE,
		SUCCESS,
		FAIL
	}

	public abstract class StellarNode : MonoBehaviour {
		
		public StellarNode Parent;
		public List<StellarNode> Children;
		protected Ship cachedRoot;
		protected StellarSubroutine subRoutine;
		protected StellarSystem aiSystem;
		protected StellarStatus status = StellarStatus.IDLE;

		public virtual void Run() {
			if (status != StellarStatus.RUNNING) {
				onBegin ();
			}
		}	

		public virtual void ChildFinished(StellarStatus status) {
			if (Parent) {
				Parent.ChildFinished (status);
			}
		}

		public StellarStatus GetStatus() {
			return status;
		}

		// reinitialize things here
		protected virtual void onFinish(StellarStatus status) {
			status = StellarStatus.IDLE;
			if (Parent) {
				Parent.ChildFinished (status);
			}
		}

		// finish things up here!
		protected virtual void onBegin () {
			status = StellarStatus.RUNNING;
		}
	}
}
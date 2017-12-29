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
		
		public List<StellarNode> Children;

		protected StellarNode Parent;
		protected Ship cachedRoot;
		protected StellarSubroutine subRoutine;
		protected StellarSystem aiSystem;
		protected StellarStatus status = StellarStatus.IDLE;
		protected bool setup = false;

		protected virtual void init() {
			FindChildren ();
			setup = true;
		}
			
		public virtual void Run() {
				onBegin ();
		}	

		public virtual void ChildFinished(StellarStatus finstatus) {
			if (Parent) {
				Parent.ChildFinished (finstatus);
			}
		}

		public StellarStatus GetStatus() {
			return status;
		}
			
		public void SetParent(StellarNode parent) {
			if (!setup) {
				init ();
			}
			this.Parent = parent;
		}

		public void Register(StellarSubroutine routine){
			if (!setup) {
				init ();
			}
			subRoutine = routine;
			cachedRoot = routine.cachedRoot;
			aiSystem = routine.aiSystem;
			foreach (StellarNode child in Children) {
				child.Register (routine);
			}
		}

		public StellarNode GetParent(){
			return Parent;
		}

		// reinitialize things here
		protected virtual void onBegin () {
			status = StellarStatus.RUNNING;
		}

		// finish things up here!
		protected virtual void onFinish(StellarStatus finstatus) {
			this.status = StellarStatus.IDLE;
			if (Parent) {
				Parent.ChildFinished (finstatus);
			}
		}

		public virtual void OnInterrupt() {
			if (Parent) {
				Parent.OnInterrupt ();
			}
		}

		private void FindChildren() {
			Children.Clear ();
			for(int i = 0; i < transform.childCount; i++) {
				StellarNode node;
				if ((node = transform.GetChild (i).GetComponent<StellarNode> ())) {
					Children.Add (node);
					node.Parent = this;
				}
			}
		}
	}
}
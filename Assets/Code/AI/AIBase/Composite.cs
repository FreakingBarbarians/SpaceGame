using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : Node {
	[SerializeField]
	private int index;
	public bool AutoContinue;

	public override List<Decision> Continue () {
		// first check if we have a running child

		for (int i = 0; i < children.Count; i++) {
			if (children [i].GetStatus () == NodeStatus.RUNNING) {
				status = NodeStatus.RUNNING;
				return new List<Decision> ();
			}
		}	

		if (index >= children.Count) {
			parent.Finished (this, NodeStatus.FAIL);
			// empty list
			index = 0;
			return new List<Decision>();
		} else {
			index++;
			return this.children [index].Continue ();
		}
	}

	public override void Finished(Node node, NodeStatus status) {
		index = 0;
		if (!parent) {
			return;
		}
		if (status == NodeStatus.SUCCESS) {
			parent.Finished (this, NodeStatus.SUCCESS);
			status = NodeStatus.IDLE;
		} else if (status == NodeStatus.FAIL) {
			parent.Finished (this, NodeStatus.FAIL);
			if (AutoContinue) {
				foreach (Decision dec in Continue()) {
					source.QueueAdd (dec);
				}
			}
		}
	}

	// things get yucky with parallel i think...

}

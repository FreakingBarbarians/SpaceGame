//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Composite<T> : Node<T> {
//	[SerializeField]
//	private int index;
//	public bool AutoContinue;
//
//	public virtual List<Decision<T>> Continue () {
//		if (index >= children.Count) {
//			parent.Finished (this, NodeStatus.FAIL);
//			// empty list
//			return new List<Decision<T>>();
//		} else {
//			return this.children [index].Continue ();
//		}
//		index++;
//	}
//
//	public virtual void Finished(Node<T> node, NodeStatus status) {
//		if (status == NodeStatus.SUCCESS) {
//			parent.Finished (this, NodeStatus.SUCCESS);
//			index = 0;
//		} else if (status == NodeStatus.FAIL) {
//			parent.Finished (this, NodeStatus.FAIL);
//			if (AutoContinue) {
//				foreach (Decision<T> dec in Continue()) {
//					source.QueueAdd (dec);
//				}
//			}
//		}
//	}
//
//	// things get yucky with parallel i think...
//
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//// @TODO: T will inherit form ai_base
//public abstract class Node<T> : MonoBehaviour {
//
//	public Node<T> parent;
//	public List<Node<T>> children = new List<Node<T>>();
//	protected NodeStatus status;
//	protected BehaviorTree<T> source;
//
//	public void Start(){
//		for (int i = 0; i < gameObject.transform.childCount; i++) {
//			Node<T> node = transform.GetChild (i).GetComponent<Node<T>> ();
//			if (node) {
//				node.parent = this;
//				children.Add (node);
//				Debug.Log (gameObject.name +" Added: " + node.gameObject.name);
//			}
//		}
//	}
//
//	public virtual List<Decision<T>> Continue () {
//		return null;
//	}
//
//	public NodeStatus GetStatus(){
//		return status;
//	}
//
//	public virtual void Finished(Node<T> node, NodeStatus status) {
//		
//	}
//
//	public void SetSource(BehaviorTree<T> source){
//		this.source = source;
//		foreach (Node<T> child in children) {
//			child.SetSource (source);
//		}
//	}
//}
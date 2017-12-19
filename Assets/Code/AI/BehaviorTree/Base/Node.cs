using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @TODO: T will inherit form ai_base
public abstract class Node<T> : MonoBehaviour {
	[SerializeField]
	private Node<T> parent;
	private List<Node<T>> children = new List<Node<T>>();
	private NodeStatus status;

	public void Start(){
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			Node<T> node = transform.GetChild (i).GetComponent<Node<T>> ();
			if (node) {
				node.parent = this;
				children.Add (node);
				Debug.Log (gameObject.name +" Added: " + node.gameObject.name);
			}
		}
	}

	public virtual List<Decision<T>> Continue () {
		return null;
	}

	public NodeStatus GetStatus(){
		return status;
	}
}
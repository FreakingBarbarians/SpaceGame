using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @TODO: T will inherit form ai_base
public abstract class Node<T> : MonoBehaviour {
	[SerializeField]
	private Node<T> parent;
	private List<Node<T>> children;

	private NodeStatus status;

	public virtual List<Decision<T>> Continue () {
		return null;
	}

	public NodeStatus GetStatus(){
		return status;
	}
}
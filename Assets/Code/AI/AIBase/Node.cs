using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeStatus
{
	FAIL,
	RUNNING,
	IDLE,
	SUCCESS
}

public abstract class Node : MonoBehaviour {
	[HideInInspector]
	public Node parent;
	public List<Node> children = new List<Node>();
	public NodeStatus status = NodeStatus.IDLE;
	protected AIBase source;
	protected Ship target;

	protected virtual void init() {
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			Node node = transform.GetChild (i).GetComponent<Node> ();
			if (node) {
				node.parent = this;
				children.Add (node);
				Debug.Log (gameObject.name +" Added: " + node.gameObject.name);
			}
		}		
	}

	public virtual List<Decision> Continue () {
		return null;
	}

	public NodeStatus GetStatus(){
		return status;
	}

	public virtual void Finished(Node node, NodeStatus status) {

	}

	public void SetSource(AIBase source){
		init (); // lazy init
		this.source = source;
		// cache target ref here.
		this.target = source.target;
		foreach (Node child in children) {
			child.SetSource (source);
		}
	}
}

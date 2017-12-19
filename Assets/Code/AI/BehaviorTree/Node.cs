using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour {
	[SerializeField]
	private Node parent;
	private List<Node> children;

	public virtual List<Node> Continue () {
				
	}
}
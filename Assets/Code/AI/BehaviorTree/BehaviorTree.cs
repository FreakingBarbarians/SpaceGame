using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
// @TODO: Change where t: qvent -> where t: some_ai_class i will make some day
// @TODO: Serialize and Deserialize This Piece of SHIT :/
// @TODO: Make IUnityXmlSerializable an interface...
// @TODO: Fix this ugly mess with generic types yuck.
public class BehaviorTree<T> : QventSystem.QventHandler {

	// Time until the tree iterates over the tree from the root.
	[Range(0.2f, 60f)]
	public float TimeToCheck;

	// events used as shortcuts
	private Dictionary<QventSystem.QventType, List<Decision<T>>> shortcuts;

	[SerializeField]
	private List<Decision<T>> runningDecisions;
	private Node<T> root;

	// some abstract ai class, will contain this
	private T target;

	public BehaviorTree(T target) {
		this.target = target;
		shortcuts = new Dictionary<QventSystem.QventType, List<Decision<T>>>();
		runningDecisions = new List<Decision<T>>();
	}

	public void Process(){
		List<Decision<T>> toRemove = new List<Decision<T>>();
		List<Decision<T>> toAdd = new List<Decision<T>> ();
		foreach (Decision<T> dec in runningDecisions) {
			if(dec != null) {
				NodeStatus result = dec.Run (target, Time.fixedDeltaTime);
				switch (result) {
				case NodeStatus.SUCCESS:
					toAdd.AddRange (dec.Continue ());
					toRemove.Add (dec);
					break;
				case NodeStatus.FAIL:
					toRemove.Add (dec);
					break;
				case NodeStatus.RUNNING:
					break;
				}
			}
		}
		// remove decisions that are finished
		foreach (Decision<T> tr in toRemove) {
			runningDecisions.Remove (tr);
		}

		runningDecisions.AddRange (toAdd);
		// @TODO: Timeout processing
	}

	public void AddShortcut(QventSystem.QventType QventType, Decision<T> Consequent){
		List<Decision<T>> entry;
		if (shortcuts.ContainsKey (QventType)) {
			entry = shortcuts [QventType];
			entry.Add (Consequent);
		} else {
			entry = new List<Decision<T>> ();
			entry.Add (Consequent);
			shortcuts [QventType] = entry;
		}
	}

	public void RemoveShortcut(QventSystem.QventType QventType, Decision<T> Consequent){
		if(shortcuts.ContainsKey(QventType)){
			List<Decision<T>> entry = shortcuts [QventType];
			entry.Remove (Consequent);
		}
	}

	// @TODO: pass event down from some_ai_class
	public void HandleQvent(Qvent e) {
		if (shortcuts.ContainsKey (e.QventType)) {
			foreach (Decision<T> dec in shortcuts[e.QventType]) {
				if (dec.EventRun (target, Time.fixedDeltaTime, e.PayloadType, e.Payload) == NodeStatus.RUNNING) {
					runningDecisions.Add(dec);
				}
			}
		}
	}
}

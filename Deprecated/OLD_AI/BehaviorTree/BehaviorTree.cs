//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using QventSystem;
//// @TODO: Likely ai will have prefab.
//// @NOTE: I don't really wanna write serialization for BTrees so, we can't do things like adapt ai params in realtime.
//// Maybe one day? c:
//public class BehaviorTree<T> : MonoBehaviour, QventSystem.QventHandler {
//
//	// Time until the tree iterates over the tree from the root.
//	[Range(0.2f, 60f)]
//	public float TimeToCheck;
//
//	// events used as shortcuts
//	private Dictionary<QventSystem.QventType, List<Decision<T>>> shortcuts;
//
//	private List<Decision<T>> runningDecisions = new List<Decision<T>> ();
//	private List<Decision<T>> toRemove = new List<Decision<T>>();
//	private List<Decision<T>> toAdd = new List<Decision<T>> ();
//
//	// we use component as a yucky workaround :) to expose to unity!
//	public Component RootRef;
//	private Node<T> root;
//
//	public List<Component> groups;
//
//	// some abstract ai class, will contain this
//	private T target;
//
//	private void init() {
//		shortcuts = new Dictionary<QventSystem.QventType, List<Decision<T>>>();
//		runningDecisions = new List<Decision<T>>();
//
//		root = (Node<T>)RootRef;
//		// get groups in children
//		root.SetSource(this);
//		foreach(Component group in groups){
//			((Group<BehaviorTree<T>>)group).Register (this);
//		}
//	}
//
//	public void Register(T target){
//		this.target = target;
//	}
//
//	public void Start(){
//		init ();
//	}
//
//	public void SetTree(Node<T> root){
//		this.root = root;
//		Group<BehaviorTree<T>>[] groups = root.gameObject.GetComponentsInChildren<Group<BehaviorTree<T>>> ();
//		foreach(Group<BehaviorTree<T>> group in groups){
//			group.Register (this);
//		}
//		// @TODO: Make sure this makes sense.. the e rest...?
//	}
//
//	public void Process(){
//		foreach (Decision<T> dec in runningDecisions) {
//			if(dec != null) {
//				NodeStatus result = dec.Run (target, Time.fixedDeltaTime);
//				switch (result) {
//				case NodeStatus.SUCCESS:
//					dec.parent.Finished (dec, NodeStatus.SUCCESS);
//					toRemove.Add (dec);
//					break;
//				case NodeStatus.FAIL:
//					dec.parent.Finished (dec, NodeStatus.FAIL);
//					toRemove.Add (dec);
//					break;
//				case NodeStatus.RUNNING:
//					break;
//				}
//			}
//		}
//		// remove decisions that are finished
//		foreach (Decision<T> tr in toRemove) {
//			runningDecisions.Remove (tr);
//		}
//
//		runningDecisions.AddRange (toAdd);
//
//		toRemove.Clear ();
//		toAdd.Clear ();
//
//		// @TODO: Timeout processing
//	}
//
//	public void AddShortcut(QventSystem.QventType QventType, Decision<T> Consequent){
//		List<Decision<T>> entry;
//		if (shortcuts.ContainsKey (QventType)) {
//			entry = shortcuts [QventType];
//			entry.Add (Consequent);
//		} else {
//			entry = new List<Decision<T>> ();
//			entry.Add (Consequent);
//			shortcuts [QventType] = entry;
//		}
//	}
//
//	public void RemoveShortcut(QventSystem.QventType QventType, Decision<T> Consequent){
//		if(shortcuts.ContainsKey(QventType)){
//			List<Decision<T>> entry = shortcuts [QventType];
//			entry.Remove (Consequent);
//		}
//	}
//
//	public void HandleQvent(Qvent e) {
//		if (shortcuts.ContainsKey (e.QventType)) {
//			foreach (Decision<T> dec in shortcuts[e.QventType]) {
//				if (dec.EventRun (target, Time.fixedDeltaTime, e.PayloadType, e.Payload) == NodeStatus.RUNNING) {
//					runningDecisions.Add(dec);
//				}
//			}
//		}
//	}
//
//	// queued addition & removal
//
//	public void QueueRemove(Decision<T> DecisionToRemove){
//		if (this.toRemove.Contains (DecisionToRemove)) {
//			return;
//		} else {
//			DecisionToRemove.parent.Finished (DecisionToRemove, NodeStatus.FAIL);
//			toRemove.Add (DecisionToRemove);
//		}
//	}
//
//	public void QueueAdd(Decision<T> DecisionToAdd){
//		if(this.toAdd.Contains(DecisionToAdd)) {
//			return;
//		} else {
//			toAdd.Add(DecisionToAdd);
//		}
//	}
//}

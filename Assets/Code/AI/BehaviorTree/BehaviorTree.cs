using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
// @TODO: Change where t: qvent -> where t: some_ai_class i will make some day
public abstract class BehaviorTree<T> :  IUnityXmlSerializable, QventSystem.QventHandler where T: QventHandler {

	// Time until the tree iterates over the tree from the root.
	[Range(0.2, 60)]
	public float TimeToCheck {get; set;}

	// events used as shortcuts
	private Dictionary<QventSystem.QventType, List<Node>> shortcuts;

	[SerializeField]
	private List<Decision<T>> runningDecisions;
	private Node root;

	// some abstract ai class, will contain this
	private T target;

	public void FixedUpdate(){
		List<Decision<T>> toRemove = new List<Decision<T>>();
		List<Decision<T>> toAdd = new List<Decision<T>> ();
		foreach (Decision<T> dec in runningDecisions) {
			if(dec) {
				DecisionResult result = dec.Run (target, Time.fixedDeltaTime);
				switch (result) {
				case DecisionResult.SUCCESS:
					toAdd.AddRange (dec.Continue ());
					toRemove.Add (dec);
					break;
				case DecisionResult.FAIL:
					toRemove.Add (dec);
					break;
				case DecisionResult.RUNNING:
					break;
				}
			}
		}
		runningDecisions.RemoveRange (toRemove);
		runningDecisions.AddRange (toAdd);
	}

	// @TODO: pass event down from some_ai_class
	public void HandleEvent(Qvent e){
		if (shortcuts.ContainsKey (e.QventType)) {
			runningDecisions.AddRange (shortcuts [e.QventType]);
		}
	}

}

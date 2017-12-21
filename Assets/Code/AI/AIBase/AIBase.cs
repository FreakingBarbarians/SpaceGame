using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class AIBase : MonoBehaviour, QventHandler {

	protected Dictionary<string, float> floatAttributes = new Dictionary<string, float>();
	protected Dictionary<string, bool> boolAttributes = new Dictionary<string,bool> ();

	// Time until the tree iterates over the tree from the root.
	[Range(0.2f, 60f)]
	public float TimeToCheck;
	private float timeToCheckTimer;
	// events used as triggers
	private Dictionary<QventType, List<Decision>> triggers = new Dictionary<QventType, List<Decision>>();

	private List<Decision> runningDecisions = new List<Decision> ();
	private List<Decision> toRemove = new List<Decision>();
	private List<Decision> toAdd = new List<Decision> ();

	[SerializeField]
	private Node root;

	public List<Group> groups;

	// The Model
	// I.e. Ship or Sth like that...
	public Ship target;

	protected virtual void init() {
		// give root this, also inits tree
		root.SetSource(this);
		// get groups in children
		foreach(Group group in groups){
			group.Register (this);
		}
	}

	public void FixedUpdate(){
		Process ();
	}

	public void Start() {
		init ();
	}

	public void Process(){
		foreach (Decision dec in runningDecisions) {
			if(dec != null) {
				NodeStatus result = dec.Run (Time.fixedDeltaTime);
				switch (result) {
				case NodeStatus.SUCCESS:
					dec.parent.Finished (dec, NodeStatus.SUCCESS);
					toRemove.Add (dec);
					break;
				case NodeStatus.FAIL:
					dec.parent.Finished (dec, NodeStatus.FAIL);
					toRemove.Add (dec);
					break;
				case NodeStatus.RUNNING:
					break;
				}
			}
		}
		// remove decisions that are finished
		foreach (Decision tr in toRemove) {
			tr.status = NodeStatus.IDLE;
			runningDecisions.Remove (tr);
		}

		runningDecisions.AddRange (toAdd);

		toRemove.Clear ();
		toAdd.Clear ();

		// @TODO: Timeout processing

		if (timeToCheckTimer <= 0) {
			timeToCheckTimer += TimeToCheck;
			runningDecisions.AddRange (root.Continue ());
		}

		timeToCheckTimer -= Time.fixedDeltaTime;
	}

	public void AddTrigger(QventSystem.QventType QventType, Decision Consequent){
		List<Decision> entry;
		if (triggers.ContainsKey (QventType)) {
			entry = triggers [QventType];
			entry.Add (Consequent);
		} else {
			entry = new List<Decision> ();
			entry.Add (Consequent);
			triggers [QventType] = entry;
		}
	}

	public void RemoveTrigger(QventSystem.QventType QventType, Decision Consequent){
		if(triggers.ContainsKey(QventType)) {
			List<Decision> entry = triggers [QventType];
			entry.Remove (Consequent);
		}
	}

	public void HandleQvent(Qvent e) {
		Debug.Log ("QVENT RECVD: " + e.QventType.ToString ());
		if (triggers.ContainsKey (e.QventType)) {
			Debug.Log ("FOUND QVENT");
			foreach (Decision dec in triggers[e.QventType]) {
				
				if (dec.EventRun (Time.fixedDeltaTime, e.PayloadType, e.Payload) == NodeStatus.RUNNING) {
					runningDecisions.Add (dec);
				}
			}
		}
		// @TODO: Think about how you can combine EventRun into Process
	}

	// queued addition & removal

	public void QueueRemove(Decision DecisionToRemove, bool notify = false){
		if (this.toRemove.Contains (DecisionToRemove)) {
			return;
		} else {
			if (notify) {
				DecisionToRemove.parent.Finished (DecisionToRemove, NodeStatus.FAIL);
			}
			toRemove.Add (DecisionToRemove);
		}
	}

	public void QueueAdd(Decision DecisionToAdd){
		if(this.toAdd.Contains(DecisionToAdd)) {
			return;
		} else {
			DecisionToAdd.status = NodeStatus.RUNNING;
			toAdd.Add(DecisionToAdd);
		}
	}

	public float GetFloatAttribute(string FloatName){
		if(floatAttributes.ContainsKey(FloatName)) {
			return floatAttributes[FloatName];
		} else {
			Debug.LogWarning("No value : " + FloatName + "in AiBase of : " + gameObject.name);
			return float.MinValue;
		}
	}

	public void SetFloatAttribute(string FloatName, float Value){
		floatAttributes [FloatName] = Value;
	}

	public bool GetBoolAttribute(string BoolName){
		if(floatAttributes.ContainsKey(BoolName)) {
			return boolAttributes[BoolName];
		} else {
			Debug.LogWarning("No value : " + BoolName + "in AiBase of : " + gameObject.name);
			return false;
		}
	}

	public void SetBoolAttribute(string BoolName, bool Value){
		boolAttributes [BoolName] = Value;
	}
}

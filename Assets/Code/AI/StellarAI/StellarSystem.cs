using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

namespace StellarAI{
	public class StellarSystem : MonoBehaviour, QventHandler {

		public List<StellarSubroutine> SubRoutinesRef;

		public StellarProcess ActiveProcess;
		public Dictionary<string, StellarSubroutine> SubRoutines = new Dictionary<string, StellarSubroutine>();
		public Dictionary<QventType, StellarSubroutine> Triggers = new Dictionary<QventType, StellarSubroutine>();
		public StellarSubroutine IdleRoutine; // also a default routine
		public StellarSubroutine ActiveRoutine;
		public Ship Root;
		public List<MyQventEmitter> EventEmitters;

		public virtual void init(){
			foreach (StellarSubroutine routine in SubRoutinesRef) {
				SubRoutines.Add (routine.gameObject.name, routine);
				Triggers.Add (routine.Trigger, routine);
				routine.SetRoot (this);
				routine.SetParent (null);
			}
			foreach (MyQventEmitter qv in EventEmitters) {
				qv.RegisterListener (this);
			}
		}

		public void Start(){
			init ();
		}

		public void FixedUpdate() {
			if (ActiveProcess) {
				ActiveProcess.Process ();
			} else {
				if (ActiveRoutine) {
					ActiveRoutine.Run ();
				} else {
					GoIdle ();				
				}
			}
		}

		public void GoIdle() {
			if (!IdleRoutine) {
				return;
			}
			ActiveRoutine = IdleRoutine;
		} // switches to the idle subroutine

		public void HandleQvent(Qvent qvent){
			// Gonna need some complex event handling logic...
			if(Triggers.ContainsKey(qvent.QventType)) {

				if (ActiveProcess) {
					ActiveProcess.OnInterrupt ();
				}
				StellarSubroutine result = Triggers [qvent.QventType];
				Triggers [qvent.QventType].HandleQvent (qvent);

			}
		}
	}
}

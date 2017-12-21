using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

namespace StellarAI{
	public class StellarSystem : MonoBehaviour, QventHandler {
		public StellarProcess ActiveProcess;
		public Dictionary<string, StellarSubroutine> SubRoutines = new Dictionary<string, StellarSubroutine>();
		public Dictionary<QventType, StellarSubroutine> Triggers = new Dictionary<QventType, StellarSubroutine>();
		public StellarSubroutine IdleRoutine; // also a default routine
		public StellarSubroutine ActiveRoutine;
		public void FixedUpdate() {
			if (ActiveProcess) {
				ActiveProcess.Process ();
			} else {
				GoIdle ();
			}
		}

		public void GoIdle() {
			if (!IdleRoutine) {
				return;
			}
			ActiveProcess = IdleRoutine.Run ();
		} // switches to the idle subroutine

		public void HandleQvent(Qvent qvent){
			if(Triggers.ContainsKey(qvent.QventType)) {
				Triggers [qvent.QventType].HandleQvent (qvent);
			}
		}
	}
}

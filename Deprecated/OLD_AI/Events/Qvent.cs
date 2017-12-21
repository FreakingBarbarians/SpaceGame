using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QventSystem {

	// Extend this to include your own EventTypes
	public enum QventType {
		NONE,
		STANDARD,
		SHIP_DETECTED,
		DECISION_EVENT
	}
	
	// [EventType|PayloadType|Payload]
	public struct Qvent {
		public readonly QventType QventType;
		public readonly Type PayloadType;
		public readonly object Payload;

		public Qvent(QventType QventType){
			this.QventType = QventType;
			PayloadType = null;
			Payload = null;
		}

		public Qvent(QventType QventType, Type PayloadType, object Payload){
			this.QventType = QventType;
			this.PayloadType = PayloadType;
			this.Payload = Payload;
		}
	}
	// coolcats

	// Its up to you to queue the events if u want to.
	public interface QventHandler {
		void HandleQvent(Qvent myEvent);
	}

	// this is enough!
}
// if shit gets broke we can always refactor refactor refactor

// cases to consider

// Event A happens dispatched to objects 1 2 3 4 in order.
// 1(A) -> 3 to be destroyed
// Can't process event for 3.
// Have to do null checking :(
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qvent {

	// Extend this to include your own EventTypes
	public enum QventType {
		NONE
	}
	
	// [EventType|PayloadType|Payload]
	public struct Qvent {
		public QventType QventType;
		public Type PayloadType;
		public object Payload;

		public Qvent(QventType EventType){
			this.EventType = EventType;
			PayloadType = null;
			Payload = null;
		}

		public Qvent(QventType EventType, Type PayloadType, object Payload){
			this.EventType = EventType;
			this.PayloadType = PayloadType;
			this.Payload = Payload;
		}
	}
	// coolcats

	// Its up to you to queue the events if u want to.
	public interface QventHandler {
		void HandleEvent(Qvent myEvent);
	}

	// this is enough!
}
// if shit gets broke we can always refactor refactor refactor

// cases to consider

// Event A happens dispatched to objects 1 2 3 4 in order.
// 1(A) -> 3 to be destroyed
// Can't process event for 3.
// Have to do null checking :(
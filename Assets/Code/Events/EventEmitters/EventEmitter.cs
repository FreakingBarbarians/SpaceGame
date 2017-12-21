using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

// maybe not serializable and just initialized as a side-effect
public interface IEventEmitter {
	void RegisterListener (QventHandler Listener);

	void UnregisterListener (QventHandler Listener);
}

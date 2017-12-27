using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveShip : MonoBehaviour {
	public InputField fileinput;

	public void OnClick() {
		SpaceSerializerDeserializer.MyMonoSerializeToFile(ShipPicker.instance.GetCurrentShip().GetComponent<Ship>(), fileinput.text);
    }
}

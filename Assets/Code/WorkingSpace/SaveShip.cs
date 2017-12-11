using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveShip : MonoBehaviour {
    public void OnClick() {
        SpaceSerializerDeserializer.SerializeShipToFile(ShipPicker.instance.GetCurrentShip().GetComponent<Ship>(), "testShip.xml");
    }
}

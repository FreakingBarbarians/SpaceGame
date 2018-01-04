using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInfoPaneController : MonoBehaviour {
    public Text Name;
    public Text EnergyCapacity;
    public Text EnergyRegen;
    public Text MaxVelocity;
    public Text Acceleration;
    public Text MaxTurnVelcoity;
    public Text TurnAcceleration;

    public void Set(GameObject InfoSource) {
        Ship s;
        if ((s = InfoSource.GetComponent<Ship>())) {
            Name.text = s.BASE_NAME;
            EnergyCapacity.text = s.EnergyMax.ToString();
            EnergyRegen.text = s.EnergyRegen.ToString();
            MaxVelocity.text = s.DeltaPositionMax.ToString();
            Acceleration.text = s.DeltaPositionFactor.ToString();
            MaxTurnVelcoity.text = s.DeltaRotationMax.ToString();
            TurnAcceleration.text = s.DeltaRotationAcceleration.ToString();
        }
    }
}

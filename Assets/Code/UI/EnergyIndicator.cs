using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicator : TickingUI {

	public Text textComponent;


	override public void Refresh(){
		textComponent.text = "Energy: " + PlayerController.instance.GetShip ().EnergyCur + "/" + PlayerController.instance.GetShip ().EnergyMax;
	}
}

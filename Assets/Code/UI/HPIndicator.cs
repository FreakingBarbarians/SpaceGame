using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPIndicator : TickingUI {
	public Text textComponent;

	override public void Refresh(){
		textComponent.text = "HP: " + PlayerController.instance.GetShip ().curhp + "/"
		+ PlayerController.instance.GetShip ().maxhp;
	}
}

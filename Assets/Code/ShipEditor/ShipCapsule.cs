using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipCapsule : Capsule {
	
	public Text CostText;

	public override void SetItem (GameObject item)
	{
		base.SetItem (item);
		CostText.text = "$" + item.GetComponent<Ship> ().ScrapCost.ToString();
	}

	public override void OnClick()
	{
		if (Item)
		{
			ShipEditor.instance.ShipSelected (Item);
		}
	}
}

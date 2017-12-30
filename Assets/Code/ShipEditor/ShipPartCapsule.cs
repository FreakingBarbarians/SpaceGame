using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipPartCapsule : Capsule {

	public Text CostText;

	public override void SetItem (GameObject item)
	{
		base.SetItem (item);
		CostText.text = "$" + item.GetComponent<Module> ().ScrapCost.ToString();
	}

    public override void OnClick()
    {
        if (Item)
        {
            ShipEditor.instance.ModuleSelected(Item);
        }
    }

}

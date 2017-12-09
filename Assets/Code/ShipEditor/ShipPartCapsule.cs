using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartCapsule : Capsule {

    public override void OnClick()
    {
        if (Item)
        {
            ShipEditor.instance.OnCapsuleClicked(Item);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModule : Module {

    private Ship rootShip;

    public void RegisterShip(Ship ship) {
        this.rootShip = ship;
    }

    public override void Die()
    {
        operational = false;
        // other stuff
    }
}

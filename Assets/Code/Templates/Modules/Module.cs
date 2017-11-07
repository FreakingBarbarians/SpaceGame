using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : Damageable {
    public Port.PortType type;
    public bool operational = true;
    private Port root;

    public void Register (Port port) {
        this.root = port;
    }

    public override void Die()
    {
        operational = false;
        // set sprite accordingly.
        // explodey effect
    }

    public void OnConnect() {
        // make CONNECTIONS
    }

    public void OnDisconnect() {
        // destroy CONNECTIONS
    }
}

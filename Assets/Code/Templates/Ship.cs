using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Set this script to execute before port
public class Ship : Damageable {
    public float maxSpeed;
    public float curSpeed;

    public List<Port> ports = new List<Port>();
    public List<Port> mainPorts = new List<Port>();

    private void Start()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Ship");

        // register all of our ports
        foreach (Port p in ports) {
            p.Register(this);
        }

        foreach (Port p in mainPorts) {
            p.Register(this);
        }
    }

    public void Refresh() {
        // recalculate
        // calculate health of the ship from main port module's

        int hpSum = 0;
        foreach (Port p in mainPorts) {
            if (p.IsConnected()) {
                MainModule mainMod = (MainModule) p.GetModule();
                mainMod.RegisterShip(this);
            }
        }
    }

    public void OnMainPortConnected(MainModule mod) {
        maxhp += mod.maxhp;
        curhp += mod.curhp;
    }

    public void OnPortConnected(Module mod) {
        // stuff
    }

    public void OnMainPortDisconnected(MainModule mod) {
        maxhp -= mod.maxhp;
        if (curhp >= maxhp) {
            curhp = maxhp;
        }
    }

    public void OnPortDisconnected(Module mod) {
        // more stuff
    }
}

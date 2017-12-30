using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Script should execute after Ship and before Module
public class Port : MonoBehaviour {

    public static PortType StringToPortType(string portType) {
        string s = portType.ToLower();
        if (PortType.MAIN.ToString().ToLower().Equals(s)) {
            return PortType.MAIN;
        } else if (PortType.SMALL.ToString().ToLower().Equals(s)) {
            return PortType.SMALL;
        }

        return PortType.SMALL;
    }

    public enum PortType {
        SMALL,
        MAIN
    };

    public PortType type;

	[SerializeField]
    private Module module;

    public Ship root;

    public bool IsConnected() {
        return (module != null);
    }

    public bool Connect(Module module) {

        if (module.portType != type) {
            Debug.LogWarning("Type Mismatch: " + module.portType.ToString() + "|" + gameObject.name + " " + module.gameObject.name + "|" + type.ToString());
			return false;
        }

        if (module == null) {
            Debug.LogWarning("Attempted to connect null module");
            return false;
        }

        if (this.module != null) {
            Debug.LogWarning("Port: " + gameObject.name + " Already has module connected: " + module.name);
            return false;
        }

        this.module = module;

        module.transform.position = transform.position;
		module.transform.rotation = transform.rotation;
        module.transform.SetParent(transform);

		float zpos = 0;
		if (transform.localPosition.y <= 0) {
			zpos = transform.localPosition.y;
		} else {
			zpos = 2 * transform.localPosition.y;
		}

        module.transform.localPosition = new Vector3
            (module.transform.localPosition.x,
            module.transform.localPosition.y,
            zpos/10f);	
		

        module.Register(this);
        module.OnConnect();
        root.OnPortConnected(module);
        return true;
    }

    public void Disconnect() {
        if (module == null) {
            return;
        }

        module.transform.SetParent(null);
        module.Register(null);

        module.OnDisconnect();
        root.OnPortDisconnected(module);
        module = null;
    }

	public void Eject(){
		if (module == null) {
			return;
		}

		module.transform.SetParent (null);
		module.SetAdrift ();
		module.Register (null);
		module.OnDisconnect ();

        root.OnPortDisconnected(module);
        // Module stuff.
    }

    public void Register(Ship ship) {
        this.root = ship;
		if (module) {
			module.faction = ship.faction;
		}
    }

    public Module GetModule() {
        return module;
    }
}

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
    private Module module;

    [NonSerialized]    
    private Ship root;

    public bool IsConnected() {
        return (module != null);
    }

    public bool Connect(Module module) {

        if (module.portType != type) {
            Debug.LogWarning("Type Mismatch");
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
        module.transform.SetParent(transform);
        module.Register(this);

        module.OnConnect();
        if (type == PortType.MAIN)
        {
            root.OnMainPortConnected(module);
        }
        else if (type == PortType.SMALL) {
            root.OnPortConnected(module);
        }
        return true;
    }

    public void Disconnect() {
        if (module == null) {
            return;
        }

        module.transform.SetParent(null);
        module.Register(null);

        module.OnDisconnect();

		if (type == PortType.MAIN)
		{
			root.OnMainPortDisconnected (module);
		}
		else if (type == PortType.SMALL) {
			root.OnMainPortDisconnected (module);
		}
    }

	public void Eject(){
		if (module == null) {
			return;
		}

		module.transform.SetParent (null);
		module.SetAdrift ();
		module.Register (null);
		module.OnDisconnect ();

		if (type == PortType.MAIN)
		{
			root.OnMainPortDisconnected (module);
		}
		else if (type == PortType.SMALL) {
			root.OnMainPortDisconnected (module);
		}
		// Module stuff.
	}

    public void Register(Ship ship) {
        this.root = ship;   
    }

    public Module GetModule() {
        return module;
    }
}

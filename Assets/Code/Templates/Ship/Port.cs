﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script should execute after Ship and before Module
public class Port : MonoBehaviour {

    public enum PortType {
        SMALL,
        MAIN
    };

    public PortType type;
    private Module module;
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
        root.Refresh();
        return true;
    }

    public void Disconnect() {
        if (module == null) {
            return;
        }

        module.transform.SetParent(null);
        module.Register(null);

        module.OnDisconnect();
        root.Refresh();
    }

    public void Register(Ship ship) {
        this.root = ship;   
    }

    public Module GetModule() {
        return module;
    }
}

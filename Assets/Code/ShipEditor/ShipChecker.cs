using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipChecker : MonoBehaviour {

    public static ShipChecker instance;

    void Start() {
        if (instance) {
            Debug.LogWarning("More than one instance of Ship Checker");
            return;
        }
        instance = this;
    }

    private bool CheckCockpit(bool Notify = true) {
        foreach (Port p in PlayerData.instance.PlayerShip.mainPorts) {
            Module m;
            if ((m = p.GetModule())) {
                if (m.moduleType == Module.ModuleType.COCKPIT) {
                    return true;
                }
            }
        }

        // some warning
        if (Notify)
        {
            NotificationController.instance.AddNotification("Ship must have a cockpit.");
        }
            return false;
    }

    public bool CheckShip(bool Notify = true) {
        if (!CheckCockpit(Notify)) {
            return false;
        }
        return true;
    }
}

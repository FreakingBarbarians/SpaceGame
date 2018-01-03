using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartInfoController : MonoBehaviour {

    public static PartInfoController instance;

    private void Start()
    {

        if (instance) {
            Debug.LogWarning("More than one Part Info Controller Instance");
            return;
        }

        instance = this;
    }

    public StandardModuleInfoPaneController StandardModule;
    public WeaponModuleInfoPaneController WeaponModule;
    public ShipInfoPaneController Ship;

    public void Set(GameObject InfoSource) {
        StandardModule.gameObject.SetActive(false);
        WeaponModule.gameObject.SetActive(false);
        Ship.gameObject.SetActive(false);

        if (InfoSource == null) {
            return;
        }

        Ship s;
        if ((s = InfoSource.GetComponent<Ship>())) {
            Ship.Set(InfoSource);
            Ship.gameObject.SetActive(true);
        }

        Module m;

        if ((m = InfoSource.GetComponent<Module>())) {
            if (m is ShotWeapon)
            {
                WeaponModule.Set(InfoSource);
                WeaponModule.gameObject.SetActive(true);
            }
            else {
                StandardModule.Set(InfoSource);
                StandardModule.gameObject.SetActive(true);
            }
        }
    }
}

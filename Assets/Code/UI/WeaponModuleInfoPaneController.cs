using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponModuleInfoPaneController : MonoBehaviour {
    public Text Name;
    public Text PortType;
    public Text Hitpoints;
    public Text EnergyCost;
    public Text FireRate;
    public Text Damage;
    public Text Velocity;

    public void Set(GameObject InfoSource) {

        ShotWeapon w;

        if ((w = InfoSource.GetComponent<ShotWeapon>())) {
            Name.text = w.BASE_NAME;

            PortType.text = w.portType.ToString();
            Color c = Color.white;
            switch (w.portType)
            {
                case Port.PortType.SMALL:
                    c = Color.red;
                    break;
                case Port.PortType.MAIN:
                    c = Color.cyan;
                    break;
            }

            PortType.color = c;
            Hitpoints.text = w.maxhp.ToString();
            EnergyCost.text = w.energyCost.ToString();
            FireRate.text = (1f / w.cooldown).ToString();
            Damage.text = w.Damage.ToString();
            Velocity.text = w.Velocity.ToString();
        }

    }
}

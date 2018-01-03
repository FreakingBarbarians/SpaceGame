using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StandardModuleInfoPaneController : MonoBehaviour {

    public Text Name;
    public Text PortType;
    public Text Hitpoints;
    public Text EnergyCapacity;
    public Text EnergyRegen;

    public void Set(GameObject InfoSource) {
        Module m;
        if ((m = InfoSource.GetComponent<Module>()))
        {
            Name.text = m.BASE_NAME;
            PortType.text = m.portType.ToString();

            Color c = Color.white;
            switch (m.portType) {
                case Port.PortType.SMALL:
                    c = Color.red;
                    break;
                case Port.PortType.MAIN:
                    c = Color.cyan;
                    break;
            }

            PortType.color = c;
            Hitpoints.text = m.maxhp.ToString();
            EnergyCapacity.text = m.EnergyMax.ToString();
            EnergyRegen.text = m.EnergyRegen.ToString();
        }
        else {
            Debug.LogWarning("No module script in object given to StandardModuleInfoPanelController");
        }
    }
}

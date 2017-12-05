using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;

public class ModuleBuilder {
// ALL modules have these fields \\
    public Vector3 pos;
    public Quaternion rot;
    public Module.ModuleType moduleType = Module.ModuleType.Standard;
    public Port.PortType portType = Port.PortType.SMALL;
    public bool operational = true;
    public bool adrift = false;

    public int energyCap;
    public int energyRegen;
    public float thrustPower;

    // Weapon Specific Components \\    
    public int bitMask;
    


    public void Mould(Module target) {

    }
}
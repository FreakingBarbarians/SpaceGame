using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;

// unused as of now

public class ModuleBuilder {
// ALL modules have these fields \\
    public Module.ModuleType moduleType = Module.ModuleType.STANDARD;
    public Port.PortType portType = Port.PortType.SMALL;
    public bool operational = true;
    public bool adrift = false;

    public int curHP;
    public int maxHP;

    public int energyCap;
    public int energyRegen;
    public float thrustPower;

    // oh god this is ugly.

    // Weapon Specific Components \\    
    public Weapon.WeaponType Weapon_WeaponType = Weapon.WeaponType.BASE;
    public int Weapon_BitMask;

    // Shot Weapon Specific:
    public float Weapon_Shot_Cooldown;
    public float Weapon_Shot_CooldownTimer;
    public int Weapon_Shot_EnergyCost;

    // Single Shot Weapon:
    public int Weapon_Shot_Single_Damage;
    public float Weapon_Shot_Single_Velocity;


    public void Mould(Module target) {
        target.moduleType = moduleType;
        target.portType = portType;
        target.maxhp = maxHP;
        target.curhp = curHP;
        target.EnergyMax = energyCap;
        target.EnergyRegen = energyRegen;
        target.operational = operational;
        target.adrift = adrift;

        switch (moduleType) {
            case Module.ModuleType.STANDARD:
                break;
            case Module.ModuleType.WEAPON:
                Weapon weap = (Weapon)target;
                weap.WeaponMask = Weapon_BitMask;
                weap.weaponType = Weapon_WeaponType;
                MouldWeapon(weap, weap.weaponType);
                break;
            default:
                throw new System.Exception("No matching module type");
        }
    }

    private void MouldWeapon(Weapon weapon, Weapon.WeaponType type) {
        switch (type) {
            case Weapon.WeaponType.BASE:
                break;
            case Weapon.WeaponType.SINGLE_SHOT_WEAPON:
                SingleShotWeapon ssw = (SingleShotWeapon)weapon;
                ssw.Damage = Weapon_Shot_Single_Damage;
                ssw.Velocity = Weapon_Shot_Single_Velocity;
                ssw.energyCost = Weapon_Shot_EnergyCost;
                ssw.cooldown = Weapon_Shot_Cooldown;
                ssw.cooldownTimer = Weapon_Shot_CooldownTimer;
                break;
            default:
                throw new System.Exception("No matching weapon type");
        }
    }

}
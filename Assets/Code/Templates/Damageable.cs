using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    [Range (0, 9999)]
    public int maxhp;

    [Range(0, 9999)]
    public int curhp;

    public bool invincible = false;

    public virtual void DoDamage(int amt) {

        if (invincible || amt < 0) {
            return;
        }

        curhp = (curhp - amt <= 0)? 0 : curhp - amt;

        if (curhp <= 0) {
            Die();
        }
    }

    public virtual void DoHeal(int amt) {
        curhp = (curhp + amt > maxhp) ? maxhp : curhp + amt;
    }

    public virtual void Die() {
        // to be overridden
        throw new NotImplementedException("Function Die in " + this.GetType().Name + " not Implemented");
    }

}

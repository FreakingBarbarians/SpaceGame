using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPrototype : MonoBehaviour {

    public float Drot;
    public float DrotMax;
    public float DrotFactor;

    public Vector2 Dpos;
    public float DposMax;
    public float DposFactor;

    public void FixedUpdate()
    {
        // min of Drot and DrotMax 
        transform.position += (Vector3) Dpos * Time.deltaTime;
    }

    public void Thrust(Vector2 thrustDir) {
        Dpos += thrustDir.normalized * Time.deltaTime * DposFactor;
        if (Dpos.magnitude > DposMax) {
            Dpos = Dpos.normalized * DposMax;
        }
    }

    public void Rotate(float dir) {
        Drot += dir * DrotFactor * Time.deltaTime;
        if (Mathf.Abs(Drot) > DrotMax) {
            if (Drot < 0)
            {
                Drot = -DrotMax;
            }
            else {
                Drot = DrotMax;
            }
        }
    }

    public void RotateTowards(Vector2 target) {
        transform.up = Vector3.RotateTowards(transform.up, target, DrotMax*3.141f/180f * Time.deltaTime, 0.0f);
    }
}

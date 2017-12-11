using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeController : MonoBehaviour {

    public ShipPrototype prototype;

    public bool MouseRot;

    public void Update()
    {
        Vector2 MoveVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            MoveVector.y++;
        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveVector.y--;
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveVector.x--;
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveVector.x++;
        }

        prototype.Thrust(MoveVector);

        if (Input.GetKey(KeyCode.E)) {
            prototype.Rotate(-1);
        }

        if (Input.GetKey(KeyCode.Q)) {
            prototype.Rotate(1);
        }

        if (MouseRot) {
            
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            prototype.RotateTowards(mousepos - (Vector2)prototype.transform.position);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerController : MonoBehaviour {
    public
    enum ControlMode
    {
        MouseTurn,
        MouseAim
    }


    public static PlayerController instance;

    public KeyCode UP = KeyCode.W;
    public KeyCode DOWN = KeyCode.S;
    public KeyCode LEFT = KeyCode.A;
    public KeyCode RIGHT = KeyCode.D;

    public KeyCode ROTATE_LEFT = KeyCode.Q;
    public KeyCode ROTATE_RIGHT = KeyCode.E;

    public KeyCode BRAKE = KeyCode.LeftControl;

	public KeyCode REPAIR = KeyCode.R;

    public KeyCode WEAPON_UP = KeyCode.UpArrow;
    public KeyCode WEAPON_DOWN = KeyCode.DownArrow;
    public KeyCode WEAPON_LEFT = KeyCode.LeftArrow;
    public KeyCode WEAPON_RIGHT = KeyCode.RightArrow;

    public KeyCode FIRE1 = KeyCode.Space;
    public KeyCode FIRE2 = KeyCode.LeftShift;
    public KeyCode FIRE3 = KeyCode.LeftControl;
    public KeyCode FIRE4 = KeyCode.RightControl;

    public int MASK1 = 1;
    public int MASK2 = 2;
    public int MASK3 = 4;
    public int MASK4 = 8;

    public ControlMode MODE = ControlMode.MouseAim;

    private Ship ship;

	private Vector2 weaponVector = new Vector2 (0, 0);
	private bool weaponDirty = false;
	private bool fcDirty = false;

	void Start(){
		ship = GetComponent<Ship> ();
		if (instance != null) {
			Debug.LogWarning ("Player Controller non-unique instance");
		}
		instance = this;
	}

	public void Update(){

		movementControl ();
		weaponControl ();
		fireControl ();
		SystemsControl ();
	}

	private void fireControl(){
		int totalMask = 0;

		if (Input.GetKey(FIRE1)) {
			totalMask = totalMask | MASK1;
		}

		if(Input.GetKey(FIRE2)){
			totalMask = totalMask | MASK2;
		}

		if (Input.GetKey (FIRE3)) {
			totalMask = totalMask | MASK3;
		}

		if (Input.GetKey (FIRE4)) {
			totalMask = totalMask | MASK4;
		}
		// constantly calculate this mask

		// if any change happens
		if (
			Input.GetKeyDown (FIRE1) || Input.GetKeyUp (FIRE1) ||
		   	Input.GetKeyDown (FIRE2) || Input.GetKeyUp (FIRE2) ||
		   	Input.GetKeyDown (FIRE3) || Input.GetKeyUp (FIRE3) ||
		   	Input.GetKeyDown (FIRE4) || Input.GetKeyUp (FIRE4)) {
			fcDirty = true;
		}

		// assuming this is an expensive operation so we don't wanna do it all the time.
		if (fcDirty) {
			foreach (Weapon wep in ship.weapons) {
				wep.UpdateWeaponState (totalMask);
			}
		}	

		fcDirty = false;
	}

	private void movementControl(){
		Vector2 moveVector = new Vector2 (0, 0);

		if (Input.GetKey (UP)) {
			moveVector.y++;
		}

		if (Input.GetKey (DOWN)) {
			moveVector.y--;
		}

		if (Input.GetKey (LEFT)) {
			moveVector.x--;
		}

		if (Input.GetKey (RIGHT)) {
			moveVector.x++;
		}

        if (Input.GetKey(ROTATE_LEFT))
        {
            ship.Rotate(1);
        }
        else if (Input.GetKey(ROTATE_RIGHT))
        {
            ship.Rotate(-1);
        }

        if (Input.GetKey(BRAKE)) {
            ship.Brake();
        }

        ship.Thrust(moveVector);
	}

	private void weaponControl(){
        if (MODE == ControlMode.MouseAim)
        {
            mouseWeaponControl();
        }
        else
        {
            kbWeaponControl();
        }

		weaponDirty = false;
	}
    private void kbWeaponControl() {
        if (Input.GetKeyDown(WEAPON_UP))
        {
            weaponVector.y++;
            weaponDirty = true;
        }
        if (Input.GetKeyUp(WEAPON_UP))
        {
            weaponVector.y--;
            weaponDirty = true;
        }
        if (Input.GetKeyDown(WEAPON_DOWN))
        {
            weaponVector.y--;
            weaponDirty = true;
        }
        if (Input.GetKeyUp(WEAPON_DOWN))
        {
            weaponVector.y++;
            weaponDirty = true;
        }

        // Weapon Left/Right
        if (Input.GetKeyDown(WEAPON_LEFT))
        {
            weaponVector.x--;
            weaponDirty = true;
        }
        if (Input.GetKeyUp(WEAPON_LEFT))
        {
            weaponVector.x++;
            weaponDirty = true;
        }
        if (Input.GetKeyDown(WEAPON_RIGHT))
        {
            weaponVector.x++;
            weaponDirty = true;
        }
        if (Input.GetKeyUp(WEAPON_RIGHT))
        {
            weaponVector.x--;
            weaponDirty = true;
        }

        if (weaponDirty)
        {
            // up is gun facing
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, weaponVector);

            foreach (Weapon wep in ship.weapons)
            {
                wep.transform.rotation = rot;
            }

        }
    }
    
	private void mouseWeaponControl() {
        weaponVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        weaponDirty = true;

        if (weaponDirty)
        {
			ship.PointWeaponsTowards (weaponVector);
        }
    }

	private void SystemsControl() {

		if (Input.GetKeyDown (REPAIR)) {
			if (ship.State == Ship.ShipState.REPAIR) {
				ship.StopRepair ();
			} else {
				ship.BeginRepair ();
			}
		}

	}

	public Ship GetShip(){
		return ship;
	}
}

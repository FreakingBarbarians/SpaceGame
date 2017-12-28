using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPicker : MonoBehaviour {

    public static ShipPicker instance;

    public List<GameObject> Source;
	// Use this for initialization

	void Start () {
        if (instance) {
            Debug.LogWarning("More than one Ship Picker");
            return;
        }
        instance = this;
	}

	public void SetShip(GameObject Ship) {
		// to do
	}

    public GameObject GetCurrentShip() {
		return PlayerData.instance.PlayerShip.gameObject;
    }
}

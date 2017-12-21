﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPicker : MonoBehaviour {

    public static ShipPicker instance;

    public List<GameObject> Source;
    public GameObject ShipSpawnPos;
    private List<GameObject> InstancedObjects;

    int chosen = -1;

	// Use this for initialization

	void Start () {
        if (instance) {
            Debug.LogWarning("More than one Ship Picker");
            return;
        }
        instance = this;

        InstancedObjects = new List<GameObject>();
        foreach (GameObject go in Source) {
            GameObject inst = Instantiate(go);
            inst.SetActive(false);
            inst.transform.position = ShipSpawnPos.transform.position;
            InstancedObjects.Add(inst);
        }

        if (InstancedObjects.Count >= 1) {
            InstancedObjects[0].SetActive(true);
            chosen = 0;
        }
	}

	public void NextShip() {
		InstancedObjects [chosen].SetActive (false);
		chosen++;
		if (chosen >= InstancedObjects.Count) {
			chosen = InstancedObjects.Count - 1;
		}
		InstancedObjects [chosen].SetActive (true);
	}

	public void PreviousShip(){
		InstancedObjects [chosen].SetActive (false);
		chosen--;
		if (chosen <= 0) {
			chosen = 0;
		}
		InstancedObjects [chosen].SetActive (true);
	}

    public GameObject GetCurrentShip() {
        if (chosen == -1) {
            return null;
        }
        return InstancedObjects[chosen];
    }
}
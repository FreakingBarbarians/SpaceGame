﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletPoolManager : MonoBehaviour {

	public static BulletPoolManager instance;
	public List<GameObject> ToPool;
	public Dictionary<string, UnityObjectPool<Bullet>> mapping = new Dictionary<string, UnityObjectPool<Bullet>>();

	void Start(){
		for (int i = 0; i < ToPool.Count; i++) {

			if (instance != null) {
				Debug.Log ("Singleton violated for BulletPoolManager");
				Destroy (this);
			}

			instance = this;

			List<Bullet> list = new List<Bullet> ();

			for (int x = 0; x < 100; x++) {
				GameObject go = Instantiate (ToPool [i]);
				list.Add (go.GetComponent<Bullet> ());
				go.SetActive (false);
				go.transform.SetParent (this.transform);
			}

			UnityObjectPool<Bullet> pool = new UnityObjectPool<Bullet> (list.ToArray());
			mapping.Add (ToPool[i].GetComponent<Bullet>().UNIQUE_NAME, pool);
		}
	}
		
	public Bullet Get(string id){
		return mapping[id].Get();
	}

	public void Free(string id, Bullet tofree){
		mapping [id].Free (tofree);
	}

	// need a wrapper.
}
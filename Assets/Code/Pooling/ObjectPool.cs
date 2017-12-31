using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/*  
    Generic pool
    User is responsible for initialization/setting up of
    Objects requested from the pool
*/

public class UnityObjectPool<T> {

	public GameObject prefab;
	public T[] pool;                            		// the pool
	public Dictionary<T, int> indexlookup;     			// lookup for index of objects
	protected int size;                         		// num objects in pool
	protected int front;                       		 	// front of free objects
	protected int back;                         		// back of free objects
	protected int freecount;                    		// # of free objects in pool

	public UnityObjectPool(int size){
		pool = new T[size];
		indexlookup = new Dictionary<T, int>();
		freecount = size;

		for (int i = 0; i < size; i++) {
			pool [i] = GameObject.Instantiate (prefab).GetComponent<T> ();
			indexlookup.Add(pool[i], i);
		}

		front = 0;
		back = size - 1;
	}

	public UnityObjectPool(int size, params System.Object[] args){
		pool = new T[size];
		indexlookup = new Dictionary<T, int>();
		freecount = size;

		for (int i = 0; i < size; i++) {
			pool [i] = GameObject.Instantiate (prefab).GetComponent<T> ();
			indexlookup.Add(pool[i], i);
		}

		front = 0;
		back = size - 1;
	}

	public UnityObjectPool(T[] items) {
		pool = items;
		indexlookup = new Dictionary<T, int>();
		size = pool.Length;
		freecount = size;

		for (int i = 0; i < size; i++)
		{
			indexlookup.Add(pool[i], i);
		}

		front = 0;
		back = size - 1;
	}

	public T Get() {
		if (freecount < 1) {
			throw new System.Exception("Pool is empty");
		}

		T returnable = pool[front];

		front = (front + 1) % pool.Length;

		freecount--;
		return returnable;
	}

	public void Free(T Object) {
		
		if (freecount == size) {
			throw new System.Exception("Pool Full: " + indexlookup[Object]);
		}

		// move back one space forward
		back = (back + 1) % pool.Length;
		// create a pointer to the in-use object at back
		T p = pool[back];
		// replace in-use object at back with in-comming free object
		pool[back] = Object;
		// replace the old in-use position in-comming object had with p
		pool[indexlookup[Object]] = p;

		// update index lookup to swap the positions of the two
		int t = indexlookup[Object];
		indexlookup[Object] = back;
		indexlookup[p] = t;

		freecount++;
	}
}
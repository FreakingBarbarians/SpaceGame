using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Utils {


	public static T interpolateEntry<T>(T[] source, float cur, float max){

		if (cur >= max) {
			return source [source.Length - 1];
		}

		if (cur <= 0) {
			return source [0];
		}

		return source [(int)((cur / max) * (source.Length - 1))];
	}

	public static T getRandomEntry<T>(List<T> list){
		if (list.Count < 1) {
			return default(T);
		}
		return list [UnityEngine.Random.Range(0, list.Count)];
	}

	public static T getRandomEntry<T>(T[] array){
		if (array.Length < 1) {
			return default(T);
		}
		return array [UnityEngine.Random.Range (0, array.Length)];
	}

	public static T getWeightedEntry<T>(List<T> entries, List<float> weights){
		// Use a better method some day
		if (entries.Count != weights.Count || entries.Count < 1) {
			Debug.Log ("WTF THE LISTS ARENT THE SAME LENGTH U ASSHOLE");
			return default(T);
		}

		float sum = 0;
		foreach (float f in weights) {
			sum += f;
		}

		float choose = UnityEngine.Random.value * sum;

		T last = entries [0];
		for (int i = 0; i < entries.Count; i++) {
			if (weights [i] <= choose) {
				last = entries [i];
			} else {
				break;
			}
		}
		return last;
	}

	public static bool Rollf(float samplespace, float domain) {
		float chance = samplespace / domain;
		float result = UnityEngine.Random.value;

		if (result > chance) {
			return true;
		}

		return false;
	}
}
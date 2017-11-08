using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damager : MonoBehaviour {
	[Range(1,9999)]
	public int damage;
}

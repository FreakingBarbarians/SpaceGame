using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FloatingNumber : MonoBehaviour {

	public Color startingColor;
	public Color endColor;
	public float duration;
	public float upVelocity;
	public Text imgText;
	public string text;
	private float timer = 0;

	void Start () {
		imgText.text = text;
		imgText.color = startingColor;
	}

	// Update is called once per frame
	void Update () {
		Color c = startingColor + (endColor - startingColor) * timer / duration;
		imgText.color = c;
		transform.position += Vector3.up * upVelocity * Time.deltaTime;
		if (timer >= duration) {
			Destroy (gameObject);
		}
		timer += Time.deltaTime;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrapCounter : MonoBehaviour {
	public Text text;

	void Update () {
		text.text = "Scrap: " + PlayerData.instance.GetScrap ().ToString();
	}
}

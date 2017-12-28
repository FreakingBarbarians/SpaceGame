using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetManager : MonoBehaviour {
	public GameObject FloatingSchematic;
	public GameObject FreeFloatingScrap;
	public GameObject HealthBar;
	public GameObject FloatingNumber;

	public static WidgetManager instance;

	public void Start() {
		if (instance) {
			Debug.LogWarning ("WOAH THERE, two floating item mangers");
			return;
		}
		instance = this;
	}

	public GameObject CreateFloatingSchematic(Module module, Vector2 pos) {
		GameObject go = GalaxyManager.SpawnWorldObject (FloatingSchematic, pos);
		FloatingSchematic ffm = go.GetComponent<FloatingSchematic> ();
		ffm.SetItem (module.BASE_PATH);
		return go;
	}

	public GameObject CreateFloatingScrap(int value, Vector2 pos) {
		GameObject go = GalaxyManager.SpawnWorldObject (FreeFloatingScrap, pos);
		FloatingScrap fs = go.GetComponent<FloatingScrap> ();
		fs.ScrapValue = value;
		return go;
	}

	public GameObject CreateHealthBar(){
		return GameObject.Instantiate (HealthBar);
	}

	public GameObject CreateFloatingNumber(Color startCol, Color endCol, float duration, float velocity, string text) {
		GameObject floaty = Instantiate (FloatingNumber);
		FloatingNumber num = floaty.GetComponent<FloatingNumber> ();
		num.duration = duration;num.upVelocity = velocity; num.text = text;
		num.startingColor = startCol;num.endColor = endCol;
		return floaty;
	}
}

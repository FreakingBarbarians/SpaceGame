using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItemManager : MonoBehaviour {
	public GameObject FloatingSchematic;
	public GameObject FreeFloatingScrap;

	public static FloatingItemManager instance;

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
}

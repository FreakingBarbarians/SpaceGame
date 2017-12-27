using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScrip : MonoBehaviour {

	public GameObject prefab;
	public GameObject prefab2;
	public TextAsset GalManSource;
	private bool run = false;


	private void Update(){

		int x = Random.Range (0, GalaxyManager.instance.Width - 1);
		int y = Random.Range (0, GalaxyManager.instance.Height - 1);

		Vector2 pos = GalaxyManager.instance.SectorToWorldPoint (new Vector2Int (x, y));;
		pos.x += Random.Range (-50, 50);
		pos.y += Random.Range (-50, 50);

		if (Input.GetKeyDown (KeyCode.Q)) {
			GalaxyManager.SpawnWorldObject (prefab, pos);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			GalaxyManager.SpawnWorldObject (prefab2, pos);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			SpaceSerializerDeserializer.MyMonoSerializeToFile (GalaxyManager.instance, "Galaxy.xml");
		}
		if (Input.GetKeyDown (KeyCode.L)) {
			SpaceSerializerDeserializer.DeserializeGalaxyManager (GalManSource.text);
		}
	}
}

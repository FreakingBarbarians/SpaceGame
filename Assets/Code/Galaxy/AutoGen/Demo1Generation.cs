using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Demo1Generation : GenerationScheme {
	public List<GameObject> Level0Objects;
	public List<GameObject> Level1Objects;
	public List<GameObject> Level2Objects;
	public List<GameObject> Level3Objects;
	public List<GameObject> Level4Objects;
	public int Range;

	public override void Generate (Sector sector)
	{
		List<GameObject> ltu;
		Vector2Int index = sector.index;
		if (index.magnitude >= 4) {
			ltu = Level4Objects;
		} else if (index.magnitude >= 3) {
			ltu = Level3Objects;
		} else if (index.magnitude >= 2) {
			ltu = Level2Objects;
		} else if (index.magnitude >= 1) {
			ltu = Level1Objects;
		} else {
			ltu = Level0Objects;
		}

		int num = Random.Range (1, Range);
		for (int i = 0; i < num; i++) {
			Vector2 pos = GalaxyManager.instance.SectorToWorldPoint (index);
			pos += Random.insideUnitCircle * GalaxyManager.instance.SectorSize / 2;
			GalaxyManager.SpawnWorldObject (Utils.getRandomEntry<GameObject> (ltu), pos);
		}
	}
}

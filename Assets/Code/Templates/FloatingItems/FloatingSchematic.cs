using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class FloatingSchematic : FloatingItem {
	public GameObject Prefab;
	public string RepresentedItem;
	public void SetItem(string resourcePath) {
		GameObject go = Resources.Load (resourcePath) as GameObject;
		if (go) {
			RepresentedItem = go.GetComponent<MyPrefab> ().BASE_NAME;
			Prefab = go;
			itemRenderer.sprite = Prefab.GetComponent<SpriteRenderer> ().sprite;
			float xscale =  itemRenderer.sprite.pixelsPerUnit / itemRenderer.sprite.textureRect.size.x;
			float yscale =  itemRenderer.sprite.pixelsPerUnit / itemRenderer.sprite.textureRect.size.y;
			float realScale = Mathf.Min (xscale, yscale);
			itemRenderer.gameObject.transform.localScale = new Vector3 (realScale, realScale, 1);
		} else {
			Debug.LogWarning ("Bad bad bad path to resource when setting floating item: " + resourcePath);
		}
	}
		
	public override void OnPickup ()
	{
		PlayerData.instance.AddModule (Prefab);
		Qvent qvent = new Qvent (QventType.REMOVED_FROM_GAME_WORLD, typeof(FloatingItem), this);
		foreach (QventHandler l in Listeners) {
			l.HandleQvent (qvent);
		}
		Destroy (gameObject);
	}
}

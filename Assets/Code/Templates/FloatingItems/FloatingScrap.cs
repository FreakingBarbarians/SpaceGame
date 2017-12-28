using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;

public class FloatingScrap : FloatingItem {
	public Sprite[] spritesheet;
	public int ScrapValue;

	void Start() {
		Sprite theChosenOne = Utils.getRandomEntry<Sprite> (spritesheet);
		itemRenderer.sprite = theChosenOne;
		float xscale =  itemRenderer.sprite.pixelsPerUnit / itemRenderer.sprite.textureRect.size.x;
		float yscale =  itemRenderer.sprite.pixelsPerUnit / itemRenderer.sprite.textureRect.size.y;
		float realScale = Mathf.Min (xscale, yscale);
		itemRenderer.gameObject.transform.localScale = new Vector3 (realScale, realScale, 1);
	}

	public override void OnPickup ()
	{
		PlayerData.instance.AddScrap (ScrapValue);
		Qvent qvent = new Qvent (QventType.REMOVED_FROM_GAME_WORLD, typeof(FloatingItem), this);
		foreach (QventHandler l in Listeners) {
			l.HandleQvent (qvent);
		}
		Destroy (gameObject);
	}
}

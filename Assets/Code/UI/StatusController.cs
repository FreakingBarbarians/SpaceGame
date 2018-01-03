using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour {

	public Text HP;
	public Text MP;
	public Sprite[] HPSprites;
	public Sprite[] MPSprites;
	public Image HPIm;
	public Image MPIm;
	public Image ShipIm;

	void Update () {
		if (!PlayerData.instance.PlayerShip) {
			return;
		}

		int hpcur = PlayerData.instance.PlayerShip.curhp;
		int hpmax = PlayerData.instance.PlayerShip.maxhp;
		int mpcur = PlayerData.instance.PlayerShip.EnergyCur;
		int mpmax = PlayerData.instance.PlayerShip.EnergyMax;

		HP.text = hpcur + "/" + hpmax;
		MP.text = mpcur + "/" + mpmax;

		HPIm.sprite = Utils.interpolateEntry<Sprite> (HPSprites, hpcur, hpmax);
		MPIm.sprite = Utils.interpolateEntry<Sprite> (MPSprites, mpcur, mpmax);

		ShipIm.sprite = PlayerData.instance.PlayerShip.GetComponent<SpriteRenderer> ().sprite;
	}
}

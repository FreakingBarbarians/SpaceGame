using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QventSystem;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour, QventHandler {

	public Damageable source;
	public Image bar;
	public Sprite[] barSprites;

	public void Refresh() {
		bar.sprite = Utils.interpolateEntry<Sprite> (barSprites, source.curhp, source.maxhp);
		if (source.curhp == source.maxhp) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
	}

	public void HandleQvent(Qvent myQvent) {
		switch (myQvent.QventType) {
		case QventType.DAMAGED:
			Refresh ();
			break;
		case QventType.STRUCTURE_CHANGED:
			Refresh ();
			break;
		case QventType.HEALED:
			Refresh ();
			break;
		}
	}
}

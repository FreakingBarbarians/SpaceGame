using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_COMBAT_TYPE {
	NONE
}

public class AIShip : AIBase {

	// other stuff i guess.
	public FACTION Faction;

	// more logic stuff as needed i suppose.
	public bool CreateAI;

	private GameObject aiBase;

	// @TODO: This might be unecessary
	public bool CreateCombat = true;
	private GameObject aiCombatLogic;
	private CircleCollider2D aiCombatCollider;
	public float aiCombatRadius;
	// lookup the proper tree.
	public AI_COMBAT_TYPE CombatAIType;

	protected override void init(){
		base.init ();
		target.SetFaction (Faction);
		if (CreateAI) {
			aiBase = new GameObject ("AI Base");
			aiBase.transform.SetParent (this.transform);
			aiBase.transform.localPosition = Vector3.zero;
			aiBase.layer = LayerMask.NameToLayer ("Ship");

			if (CreateCombat) {
				aiCombatLogic = new GameObject ("AI Combat Logic");
				aiCombatLogic.transform.SetParent (aiBase.transform);
				aiCombatLogic.transform.localPosition = Vector3.zero;
				aiCombatLogic.layer = LayerMask.NameToLayer ("ShipOnly");
				aiCombatCollider = aiCombatLogic.AddComponent<CircleCollider2D> ();
				aiCombatCollider.radius = aiCombatRadius;
				aiCombatLogic.AddComponent<OnTriggerEnter2DEvent> ().RegisterListener (this);
			}
		}
		// more logic stuff as needed i suppose.
	}
}

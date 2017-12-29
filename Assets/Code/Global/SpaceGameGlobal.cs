using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FACTION {
	PLAYER_FACTION,
	ENEMY_FACTION,
	NEUTRAL_FACTION
}

public class SpaceGameGlobal {
	public static readonly float TICK_RATE = 1f;
	public static readonly float COMBAT_COOLDOWN = 5f;

    public static FACTION StringToFaction(string faction) {

        string s = faction.ToLower();
        if (FACTION.PLAYER_FACTION.ToString().ToLower().Equals(s)) {
            return FACTION.PLAYER_FACTION;
        } else if (FACTION.ENEMY_FACTION.ToString().ToLower().Equals(s)) {
            return FACTION.ENEMY_FACTION;
        } else if (FACTION.NEUTRAL_FACTION.ToString().ToLower().Equals(s)) {
            return FACTION.NEUTRAL_FACTION;
        }

        return FACTION.NEUTRAL_FACTION;
    }
}

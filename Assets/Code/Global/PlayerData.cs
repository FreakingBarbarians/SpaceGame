using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using QventSystem;

public class PlayerData : IUnityXmlSerializable {

	public static PlayerData instance;
	public int Scrap = 0;
	// stores paths to the thing instead. yeah? YEAH
	public Dictionary<string, GameObject> KnownModules = new Dictionary<string, GameObject> ();

	public void Start() {
		if (instance) {
			Debug.LogWarning ("More than one instance of PlayerData");
			return;
		}
		instance = this;
	}

	public int GetScrap(){
		return Scrap;
	}

	public void AddScrap(int scrapAmount){
		Scrap += scrapAmount;
	}

	public bool RemoveScrap(int scrapAmount) {
		if (Scrap >= scrapAmount) {
			Scrap -= scrapAmount;
			return true;
		}
		return false;
	}

	public void AddModule(string resourcePath){
		GameObject go = Resources.Load (resourcePath) as GameObject;
		if (go) {
			AddModule (go);
		} else {
			Debug.LogWarning ("Bad path in Adding Module > Player Data: " + resourcePath);
		}
	}

	public void AddModule(GameObject prefab) {
		Module module;
		if (!(module = prefab.GetComponent<Module> ())) {
			Debug.LogWarning ("Not a module! " + prefab.name);	
		}
		KnownModules [module.BASE_PATH] = prefab;
		Debug.Log ("Added " + module.BASE_PATH);
	}

	public void RemoveModule(GameObject prefab) {
		Module module;
		if (!(module = prefab.GetComponent<Module> ())) {
			Debug.LogWarning ("Not a module! " + prefab.name);	
		}
		if (KnownModules.ContainsKey (module.BASE_PATH)) {
			KnownModules.Remove (module.BASE_PATH);
			Debug.Log ("Removed " + module.BASE_PATH);
		}
	}

	public new static GameObject ReadXml(XmlReader reader, Component workingCO) {
		PlayerData dat = (PlayerData) workingCO;
		reader.Read ();
		dat.Scrap = int.Parse (reader.ReadString ());

		while(reader.Read() && !(!reader.IsStartElement() && reader.LocalName.Equals("KNOWN_MODULES"))) {
			if (reader.IsStartElement ()) {
				switch (reader.LocalName) {
				case "ENTRY":
					dat.AddModule (reader.ReadString ());
					break;
				}
			}
		}
		reader.Read ();
		return dat.gameObject;
	}

	public override void WriteXml (System.Xml.XmlWriter writer)
	{
		base.WriteXml (writer);
		writer.WriteStartElement ("PLAYER_DATA");

		writer.WriteElementString ("SCRAP", Scrap.ToString());

		writer.WriteStartElement ("KNOWN_MODULES");
		foreach (string knownModule in KnownModules.Keys) {
			writer.WriteElementString ("ENTRY", knownModule);
		}
		writer.WriteEndElement ();

		writer.WriteEndElement ();
	}
}

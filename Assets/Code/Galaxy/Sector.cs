using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using QventSystem;

// 100 by 100 units. This is good
public class Sector : IUnityXmlSerializable, QventHandler {

	public List<GameObject> Objects = new List<GameObject>();
	public bool Loaded = false;
	public Vector2Int index;

	public void AddToSector(GameObject Object){
		
		foreach (MonoBehaviour qventEmitter in Object.GetComponentsInChildren<MonoBehaviour>()) {
			if (qventEmitter is IQventEmitter) {
				((IQventEmitter)qventEmitter).RegisterListener (this);
			}
		}

		if(!Objects.Contains(Object)){
			Objects.Add (Object);
			Object.SetActive (Loaded);
		}
	}

	public void RemoveFromSector(GameObject Object) {
		Objects.Remove (Object);
	}

	public void Unload() {
		Loaded = false;
		foreach (GameObject obj in Objects) {
			obj.SetActive (false);
		}
	}

	public void Load() {
		Loaded = true;
		foreach (GameObject obj in Objects) {
			obj.SetActive (true);
		}
	}

	public void Check() {
		List<GameObject> toRemove = new List<GameObject> ();

		foreach (GameObject o in Objects) {
			if (!GalaxyManager.instance.WorldToSectorPoint (o.transform.position).Equals (index)) {
				GameObject obj = o;
				toRemove.Add (obj);
				GalaxyManager.instance.AddToSector (obj);
			}
		}

		foreach (GameObject o in toRemove) {
			RemoveFromSector (o);
		}
	}

	public new static GameObject ReadXml(XmlReader reader, Component workingObj)
	{
		Sector sector = (Sector)workingObj;

		reader.Read ();
		sector.Loaded = XmlUtils.DeserializeBool (reader);

		Vector2Int index = new Vector2Int (0,0);
		reader.Read ();
		index.x = int.Parse (reader.ReadString ());

		reader.Read ();
		index.y = int.Parse (reader.ReadString ());

		sector.index = index;

		return workingObj.gameObject;
	}

	public override void WriteXml (XmlWriter writer)
	{
		base.WriteXml (writer);

		writer.WriteStartElement ("SECTOR_DATA");
		XmlUtils.SerializeBool (writer, Loaded, "LOADED");
		writer.WriteElementString ("X_INDEX", index.x.ToString ());
		writer.WriteElementString ("Y_INDEX", index.y.ToString ());
		writer.WriteEndElement ();

		writer.WriteStartElement ("OBJECTS");

		if (Objects.Count >= 1) {
			foreach(GameObject obj in Objects){
				if (obj.GetComponent<IUnityXmlSerializable>()) {
					SpaceSerializerDeserializer.MyMonoSerializeToStream (writer, obj.GetComponent<IUnityXmlSerializable>());
				}
			}
		} else {
			// to leave blank space
			writer.WriteString (" ");
		}
		writer.WriteEndElement();
	}

	// QventHandler Interface Declaration

	public void HandleQvent(Qvent qvent){
		switch (qvent.QventType) {
		case QventType.DESTROYED:
			// remove destroyed ship from tracking
			if(qvent.PayloadType == typeof(Ship)){
				RemoveFromSector (((Ship)qvent.Payload).gameObject);
			}
			break;
		case QventType.REMOVED_FROM_GAME_WORLD:
			// remove destroyed object from tracking
			if(qvent.PayloadType == typeof(FloatingItem)) {
				RemoveFromSector (((FloatingItem)qvent.Payload).gameObject);
			}
			break;
		}
	}
}

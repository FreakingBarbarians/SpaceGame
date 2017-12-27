using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// 100 by 100 units. This is good
public class Sector : IUnityXmlSerializable {

	public List<GameObject> Objects = new List<GameObject>();
	public bool Loaded = false;
	public Vector2Int index;

	public void Unload() {
		foreach (GameObject obj in Objects) {
			obj.SetActive (false);
		}
	}

	public void Load() {
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
			Objects.Remove (o);	
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
}

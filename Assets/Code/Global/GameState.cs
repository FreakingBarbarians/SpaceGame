using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class GameState : IUnityXmlSerializable {
	public static GameState instance;

	[SerializeField]
	private Dictionary <IUnityXmlSerializable, IUnityXmlSerializable> Objects;

	public void Start(){
		if (instance) {
			Debug.Log ("More than One GameState Instance");
		}
		instance = this;
		Objects = new Dictionary<IUnityXmlSerializable, IUnityXmlSerializable> ();
	}

	public override void WriteXml(XmlWriter writer) { 
		writer.WriteStartElement ("OBJECTS");
		foreach (IUnityXmlSerializable Object in Objects.Keys) {
			SpaceSerializerDeserializer.MyMonoSerializeToStream (writer, Object);
		}
		writer.WriteEndElement ();
	}

	public void AddObject(IUnityXmlSerializable Object){
		Objects.Add (Object, Object);
	}

	public void RemoveObject(IUnityXmlSerializable Object){
		Objects.Remove (Object);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;

public class SpaceSerializerDeserializer : MonoBehaviour {
	public string SerializeShip(Ship ship){
		MemoryStream memstrm = new MemoryStream ();
		XmlTextWriter writer = new XmlTextWriter (memstrm, System.Text.Encoding.UTF8);
		writer.WriteStartDocument ();

		writeLiteral (writer, "name", ship.BASE_NAME);
		writeLiteral (writer, "base", ship.gameObject.name);

		writer.WriteEndDocument ();
		writer.Flush ();
		memstrm.Seek (0, SeekOrigin.Begin);
		string returnval = System.Text.Encoding.UTF8.GetString(memstrm.ToArray ());
		memstrm.Flush ();
		writer.Close ();
		return returnval;
	}

	public Ship DeserializeShip(string ship){
		return null;
	}

	public string SerializeModule(Module module){
		return null;
	}

	public Module DeserializeModule(string module){
		return null;
	}
		

	private void writeLiteral(XmlWriter writer, string name, string value){
		writer.WriteElementString (name, value);
	}

}

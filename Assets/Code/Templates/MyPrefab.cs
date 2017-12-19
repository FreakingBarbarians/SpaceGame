using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

public class MyPrefab : IUnityXmlSerializable {
	public string BASE_NAME;
	public string BASE_PATH;

    public XmlSchema GetSchema()
    {
        throw new System.NotImplementedException();
    }

    // deserialize from given xml reader
    public static GameObject ReadXml(XmlReader reader, Component workingObj)
    {
        string name = "";
        string path = "";

        reader.Read();
        name = reader.ReadString();

        reader.Read();
        path = reader.ReadString();

        GameObject prefab = Instantiate(Resources.Load(path) as GameObject);

        reader.Read();
        prefab.transform.position = XmlUtils.DeserializeVector3(reader);

        reader.Read();
        prefab.transform.rotation = XmlUtils.DeserializeQuaternion(reader);

        return prefab;
    }

    public override void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("BASE");

        writer.WriteElementString("BASE_NAME", BASE_NAME);
        writer.WriteElementString("BASE_PATH", BASE_PATH);
        XmlUtils.SerializeVector3(writer, transform.position, "POSITION");
        XmlUtils.SerializeQuaternion(writer, transform.rotation, "ROTATION");

        writer.WriteEndElement();
    }
}

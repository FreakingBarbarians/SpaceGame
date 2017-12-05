using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

public class MyMonoBehaviour : IUnityXmlSerializable {
	public string BASE_NAME;
	public string BASE_PATH;

    public XmlSchema GetSchema()
    {
        throw new System.NotImplementedException();
    }

    // deserialize from given xml reader
    public override Component ReadXml(XmlReader reader, Component workingObj)
    {
        //string name = "";
        //string path = "";

        //while (reader.Read()) {
        //    if (reader.IsStartElement()) {
        //        switch (reader.Name.ToString()) {
        //            case "BASE_NAME":
        //                name = reader.ReadString();
        //                break;
        //            case "BASE_PATH":
        //                path = reader.ReadString();
        //                break;
        //        }
        //    }
        //}

        //GameObject go = Instantiate(Resources.Load<GameObject>(path) as GameObject);
        //MyMonoBehaviour myMono = go.GetComponent<MyMonoBehaviour>();
        //return myMono;
        return workingObj;
    }

    public override void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("BASE_NAME", BASE_NAME);
        writer.WriteElementString("BASE_PATH", BASE_PATH);
        XmlUtils.SerializeVector3(writer, transform.position, "POSITION");
    }
}

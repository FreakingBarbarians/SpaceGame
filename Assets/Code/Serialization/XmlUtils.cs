using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

public class XmlUtils
{

    public static void ReadTo(XmlReader xmlReader, string name) {
        while (!xmlReader.LocalName.ToString().Equals(name))
        {
            xmlReader.Read();
        }
    }

    /*
     * Writing convention
     * Will write tag and elements
     * <Tag>
     *  <inner tag>
     *      ...
     *          Data
     *  </inner tag>
     * </Tag>
     */
    /*
     * Reading convention
     * Expects to be at starting tag
     * <TobeRead> <-- Reader
     *  Data
     * </TobeRead>
     */

    public static void SerializeVector3(XmlWriter xmlWriter, Vector3 position, string name) {
        xmlWriter.WriteStartElement(name);
        xmlWriter.WriteElementString("X", position.x.ToString());
        xmlWriter.WriteElementString("Y", position.y.ToString());
        xmlWriter.WriteElementString("Z", position.z.ToString());
        xmlWriter.WriteEndElement();
    }
    public static Vector3 DeserializeVector3(XmlReader reader) {
        Vector3 ret = new Vector3();

        reader.Read();
        ret.x = float.Parse(reader.ReadString());
        reader.Read();
        ret.y = float.Parse(reader.ReadString());
        reader.Read();
        ret.z = float.Parse(reader.ReadString());
        reader.Read();
        return ret;
    }

    public static void SerializeQuaternion(XmlWriter xmlWriter, Quaternion position, string name)
    {
        xmlWriter.WriteStartElement(name);
        xmlWriter.WriteElementString("X", position.x.ToString());
        xmlWriter.WriteElementString("Y", position.y.ToString());
        xmlWriter.WriteElementString("Z", position.z.ToString());
        xmlWriter.WriteElementString("W", position.w.ToString());
        xmlWriter.WriteEndElement();
    }
    public static Quaternion DeserializeQuaternion(XmlReader reader)
    {
        Quaternion ret = new Quaternion();

        reader.Read();
        ret.x = float.Parse(reader.ReadString());
        reader.Read();
        ret.y = float.Parse(reader.ReadString());
        reader.Read();
        ret.z = float.Parse(reader.ReadString());
        reader.Read();
        ret.w = float.Parse(reader.ReadString());
        reader.Read();
        return ret;
    }

    public static void SerializeBool(XmlWriter xmlWriter, bool value, string name) {
        xmlWriter.WriteElementString(name, value ? "TRUE" : "FALSE");
    }
    public static bool DeserializeBool(XmlReader xmlReader) {
        string val = xmlReader.ReadString();
        return val.Equals("TRUE") ?  true : false;
    }

}

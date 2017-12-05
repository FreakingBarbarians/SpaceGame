﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml.Schema;

public class SpaceSerializerDeserializer : MonoBehaviour
{
    public static string MyMonoSerializeToString(IUnityXmlSerializable thing)
    {
        MemoryStream memstrm = new MemoryStream();
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        settings.NewLineChars = "\n";
        XmlWriter xmlWriter = XmlWriter.Create(memstrm, settings);
        xmlWriter.WriteStartElement(thing.GetType().Name);

        {
            thing.WriteXml(xmlWriter);
        }

        xmlWriter.WriteEndElement();
        xmlWriter.Flush();
        memstrm.Seek(0, SeekOrigin.Begin);
        string returnval = System.Text.Encoding.UTF8.GetString(memstrm.ToArray());
        return returnval;
    }

    public static string SerializeShip(Ship ship)
    {
        MemoryStream memstrm = new MemoryStream();
        XmlSerializer ser = new XmlSerializer(typeof(Ship));
        ser.Serialize(memstrm, ship);
        memstrm.Seek(0, SeekOrigin.Begin);
        string returnval = System.Text.Encoding.UTF8.GetString(memstrm.ToArray());
        memstrm.Flush();
        return returnval;
    }

    public static void SerializeShipToFile(Ship ship, string path)
    {
        FileStream fs = new FileStream(path, FileMode.Truncate);
        byte[] bytes = Encoding.UTF8.GetBytes(MyMonoSerializeToString(ship));
        int size = bytes.Length;
        fs.Write(bytes, 0, size);
        fs.Close();
        fs.Dispose();
    }

    public static Ship DeserializeShip(string source)
    {
        using (Stream s = GenerateStreamFromString(source))
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreWhitespace = true;
            XmlReader xmlReader = XmlReader.Create(s, readerSettings);

            string name = "";
            string path = "";
            Ship ship = null;
            xmlReader.ReadStartElement();
            // get path and name
            XmlUtils.ReadTo(xmlReader, "BASE_NAME");
            name = xmlReader.ReadString();

            xmlReader.Read();
            path = xmlReader.ReadString();

            GameObject go = Instantiate(Resources.Load(path) as GameObject);
            ship = go.GetComponent<Ship>();
            ship.BASE_NAME = name;
            ship.BASE_PATH = path;

            xmlReader.Read();
            go.transform.position = XmlUtils.DeserializeVector3(xmlReader);
            return ship;
        }
    }

    // expects reader that had just read the ship tag?
    // expects ignorewhitespace to be true
    public static Ship DeserializeShip(XmlReader xmlReader) {
        string name = "";
        string path = "";
        Ship ship = null;
        xmlReader.ReadStartElement();
        // get path and name
        XmlUtils.ReadTo(xmlReader, "BASE_NAME");
        name = xmlReader.ReadString();

        xmlReader.Read();
        path = xmlReader.ReadString();

        GameObject go = Instantiate(Resources.Load(path) as GameObject);
        ship = go.GetComponent<Ship>();
        ship.BASE_NAME = name;
        ship.BASE_PATH = path;

        xmlReader.Read();
        go.transform.position = XmlUtils.DeserializeVector3(xmlReader);
        return ship;
    }
    public static string SerializeModule(Module module)
    {
        MemoryStream memstrm = new MemoryStream();
        XmlSerializer ser = new XmlSerializer(typeof(Ship));
        ser.Serialize(memstrm, module);
        memstrm.Seek(0, SeekOrigin.Begin);
        string returnval = System.Text.Encoding.UTF8.GetString(memstrm.ToArray());
        memstrm.Flush();
        return returnval;
    }

    public static void SerializeModuleToFile(Module module, string path) {
        FileStream fs = new FileStream(path, FileMode.Truncate);
        byte[] bytes = Encoding.UTF8.GetBytes(MyMonoSerializeToString(module));
        int size = bytes.Length;
        fs.Write(bytes, 0, size);
        fs.Close();
        fs.Dispose();
    }
    // expects to be on the <module> tag
    public static Module DeserializeModule(string source) {
        using (Stream s = GenerateStreamFromString(source))
        {
                // need a builder.
        }
        return null;
    }

    public static Stream GenerateStreamFromString(string s)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
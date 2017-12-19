using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml.Schema;

public class SpaceSerializerDeserializer : MonoBehaviour
{

	private class DeserializationMethod : Attribute {
		public Type SerializationType;

		public DeserializationMethod(Type SerializationType){
			this.SerializationType = SerializationType;
		}
	}

	private static Dictionary<Type, Func<IUnityXmlSerializable>> deserializers;

	static SpaceSerializerDeserializer() {
		deserializers = new Dictionary<Type,Func<IUnityXmlSerializable>> ();
		// method attribute reflection here!
	}

    public static string MyMonoSerializeToString(IUnityXmlSerializable thing)
    {
        MemoryStream memstrm = new MemoryStream();
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        settings.NewLineChars = "\n";
        XmlWriter xmlWriter = XmlWriter.Create(memstrm, settings);
        {
            MyMonoSerializeToStream(xmlWriter, thing);   
        }
        xmlWriter.Flush();
        memstrm.Seek(0, SeekOrigin.Begin);
        string returnval = System.Text.Encoding.UTF8.GetString(memstrm.ToArray());
        return returnval;
    }

    public static void MyMonoSerializeToStream(XmlWriter xmlWriter,  IUnityXmlSerializable thing) {

		if (thing is Ship) {
			xmlWriter.WriteStartElement ("SHIP");
		} else if (thing is Module) {
			xmlWriter.WriteStartElement ("MODULE");
		} else if (thing is GameState) {
			xmlWriter.WriteStartElement ("GAME_STATE");
		}
        else {
            xmlWriter.WriteStartElement("UNKOWN");
        }

        thing.WriteXml(xmlWriter);

        xmlWriter.WriteEndElement();
    }

	public static void MyMonoSerializeToFile(IUnityXmlSerializable thing, string path){
		FileStream fs = File.Create (path);
		byte[] bytes = Encoding.UTF8.GetBytes(MyMonoSerializeToString(thing));
		int size = bytes.Length;
		fs.Write(bytes, 0, size);
		fs.Close();
		fs.Dispose();
	}

	public static IUnityXmlSerializable MyMonoDeserialize(XmlReader reader){
		switch (reader.LocalName) {
		case "MODULE":
			return DeserializeModule (reader);
			break;
		case "SHIP":
			return DeserializeShip (reader);
			break;
		default:
			return null;
			break;
		}
		return null;
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
		FileStream fs = new FileStream(path, FileMode.CreateNew);
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
            XmlReader reader = XmlReader.Create(s, readerSettings);
            return DeserializeShip(reader);
        }
    }
    // expects reader that had just read the ship tag?
    // expects ignorewhitespace to be true
	[DeserializationMethod(typeof(Ship))]
    public static Ship DeserializeShip(XmlReader reader) {
        GameObject workingGO = null;
        Ship workingShip = null;
        int mainPortNum = 0;
        int portNum = 0;

        while (reader.Read() && (!reader.LocalName.Equals("SHIP")))
        {
            if (reader.IsStartElement())
            {
                Module module = null;
                // Debug.Log(reader.LocalName);
                switch (reader.LocalName)
                {
                    case "BASE":
                        workingGO = MyMonoBehaviour.ReadXml(reader, null);
                        workingShip = workingGO.GetComponent<Ship>();
                        workingShip.Start();
                        break;
                    case "DAMAGEABLE":
                        Damageable.ReadXml(reader, workingShip);
                        break;
                    case "SHIP_DATA":
                        Ship.ReadXml(reader, workingShip);
                        break;
                    case "MAIN_PORT":
                        module = DeserializeModule(reader);
                        if (module)
                        {
                            workingShip.mainPorts[mainPortNum].Connect(module);
                        }
                        mainPortNum++;
                        break;
                    case "PORT":
                        module = DeserializeModule(reader);
                        if (module)
                        {
                            workingShip.ports[portNum].Connect(module);
                        }
                        portNum++;
                        break;
                }
            }
        }
        return workingShip;
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

        GameObject workingGO = null;
        Component workingCO = null;

        using (Stream s = GenerateStreamFromString(source))
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(s, readerSettings);
            while (reader.Read()) {
                if (reader.IsStartElement()) {
                    switch (reader.LocalName) {
                        case "BASE":
                            workingGO = MyMonoBehaviour.ReadXml(reader, null);
                            workingCO = workingGO.GetComponent<Module>();
                            break;
                        case "DAMAGEABLE":
                            Damageable.ReadXml(reader, workingCO);
                            break;
                        case "MODULE":
                            Module.ReadXml(reader, workingCO);
                            break;
                        case "SINGLE_SHOT_WEAPON":
                            SingleShotWeapon.ReadXml(reader, workingCO);
                            break;
                        case "WEAPON":
                            Weapon.ReadXml(reader, workingCO);
                            break;
                        case "SHOT_WEAPON":
                            ShotWeapon.ReadXml(reader, workingCO);
                            break;
                    }
                }
            }
        }
        return (Module) workingCO;
    }

    // stops when it reads Module close tags
	[DeserializationMethod(typeof(Module))]
    public static Module DeserializeModule(XmlReader reader)
    {
        GameObject workingGO = null;
        Component workingCO = null;
        while (reader.Read() && !(!reader.IsStartElement() && reader.LocalName.Equals("MODULE")))
        {
            if (reader.IsStartElement())
            {
                switch (reader.LocalName)
                {   
                    case "BASE":
                        workingGO = MyMonoBehaviour.ReadXml(reader, null);
                        workingCO = workingGO.GetComponent<Module>();
                        break;
                    case "DAMAGEABLE":
                        Damageable.ReadXml(reader, workingCO);
                        break;
                    case "MODULE_DATA":
                        Module.ReadXml(reader, workingCO);
                        break;
                    case "SINGLE_SHOT_WEAPON":
                        SingleShotWeapon.ReadXml(reader, workingCO);
                        break;
                    case "WEAPON":
                        Weapon.ReadXml(reader, workingCO);
                        break;
                    case "SHOT_WEAPON":
                        ShotWeapon.ReadXml(reader, workingCO);
                        break;
                }
            }
        }
        return (Module)workingCO;
    }

	public static void DeserializeGameState(string source){
		using (Stream s = GenerateStreamFromString(source))
		{
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.IgnoreWhitespace = true;
			XmlReader reader = XmlReader.Create(s, readerSettings);
			DeserializeGameState(reader);
		}
	}

	// expects to be at Gamestate
	// assumes that there already is an empty gamestate object?
	[DeserializationMethod(typeof(GameState))]
	public static void DeserializeGameState(XmlReader reader) {
		while (reader.Read ()) {
			if (reader.IsStartElement ()) {
				switch (reader.LocalName) {
				case "OBJECTS":
					DeserializeGameStateObjects (reader);
					break;
				case "Homo-Erotic Encounter":
					break;
				}
			}
		}
	}

	private static void DeserializeGameStateObjects(XmlReader reader) {
		// while has readable xml, and has not reached the end tag for objects
		while(reader.Read() && !(!reader.IsStartElement() && reader.LocalName.Equals("OBJECTS"))) {
			if (reader.IsStartElement ()) {
				IUnityXmlSerializable serializable = MyMonoDeserialize (reader);
				if (serializable) {
					GameState.instance.AddObject (serializable);
				}
			}
		}
	}

    public static Stream GenerateStreamFromString(string s) {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}

// @TODO: Method attribute reflection in static constructor
// @TODO: Rejig type lookup to use System Type instead of hard-coded strings
// @TODO: Re-order some of the methods to give less headaches
// @TODO: Documentation, hah...
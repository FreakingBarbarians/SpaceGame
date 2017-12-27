using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Reflection;

public class SpaceSerializerDeserializer : MonoBehaviour
{
	
	public delegate IUnityXmlSerializable  DType(XmlReader XmlReader);

	private class DeserializationMethod : Attribute {
		public Type SerializationType;

		public DeserializationMethod(Type SerializationType){
			this.SerializationType = SerializationType;
		}
	}

	private static Dictionary<String, DType> deserializers;
	private static Dictionary<String, String> typeLookup;


	static SpaceSerializerDeserializer() {
		deserializers = new Dictionary<String, DType> ();
		typeLookup = new Dictionary<String,String> ();
		// @TODO: Improve On Reflection
		MethodInfo[] info = typeof(SpaceSerializerDeserializer).GetMethods();
		foreach (MethodInfo inf in info) {
			object[] attributes = inf.GetCustomAttributes (typeof(DeserializationMethod), true);
			if (attributes.Length >= 1) {
				DeserializationMethod deserializer = attributes [0] as DeserializationMethod;
				deserializers.Add (deserializer.SerializationType.Name, Delegate.CreateDelegate(typeof(DType), inf) as DType);
				Debug.Log ("Added Deserializer of Type " + deserializer.SerializationType.Name);
			}
		}

		// Ship Related
		typeLookup.Add (typeof(Ship).Name, typeof(Ship).Name);

		typeLookup.Add (typeof(Module).Name, typeof(Module).Name);
		typeLookup.Add (typeof(Weapon).Name, typeof(Module).Name);
		typeLookup.Add (typeof(ShotWeapon).Name, typeof(Module).Name);
		typeLookup.Add (typeof(SingleShotWeapon).Name, typeof(Module).Name);

		// TO BE REMOVED @TODO: Remove this
		typeLookup.Add (typeof(GameState).Name, typeof(GameState).Name);

		// Game State Related
		typeLookup.Add (typeof(GalaxyManager).Name, typeof(GalaxyManager).Name);

		typeLookup.Add (typeof(Sector).Name, typeof(Sector).Name);
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

		xmlWriter.WriteStartElement (typeLookup [thing.GetType ().Name]);

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

	public static IUnityXmlSerializable MyMonoDeserialize(XmlReader reader) {
		
//		switch (reader.LocalName) {
//		case typeof(Module).Name:
//			return DeserializeModule (reader);
//		case typeof(Ship).Name:
//			return DeserializeShip (reader);
//		default:
//			return null;
//		}

		DType func = deserializers[typeLookup[reader.LocalName]];
		if (func != null) {
			return func.Invoke (reader);
		} else {
			throw new Exception ("NO DESERIALIZER FOR: " + reader.LocalName);
		}
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

		while (reader.Read() && !(!reader.IsStartElement() && reader.LocalName.Equals(typeof(Ship).Name)))
        {
			Debug.Log (reader.LocalName);
            if (reader.IsStartElement())
            {
                Module module = null;
                // Debug.Log(reader.LocalName);
                switch (reader.LocalName)
                {
                    case "BASE":
                        workingGO = MyPrefab.ReadXml(reader, null);
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
			return DeserializeModule (reader);
        }
    }

    // stops when it reads Module close tags
	[DeserializationMethod(typeof(Module))]
    public static Module DeserializeModule(XmlReader reader)
    {
        GameObject workingGO = null;
        Component workingCO = null;
		while (reader.Read() && !(!reader.IsStartElement() && reader.LocalName.Equals(typeof(Module).Name)))
        {
            if (reader.IsStartElement())
            {
                switch (reader.LocalName)
                {   
                    case "BASE":
                        workingGO = MyPrefab.ReadXml(reader, null);
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
	public static GameState DeserializeGameState(XmlReader reader) {
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
		return null;
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

	// Special note: Galaxy manager is an instance singleton. So this should be set before deserialization is called. :)
	[DeserializationMethod(typeof(GalaxyManager))]
	public static GalaxyManager DeserializeGalaxyManager(XmlReader reader) { 
		GalaxyManager galman = GalaxyManager.instance;
		while (reader.Read ()) {
			if (reader.IsStartElement ()) {
				switch (reader.LocalName) {
				case "GALAXY_DATA":
					GalaxyManager.ReadXml (reader, galman);
					break;
				case "Sector":
					Sector sector = DeserializeSector (reader);
					Debug.Log (sector.index);
					galman.SetSector (sector, sector.index);
					break;
				}
			}
		}
		return galman;
	}

	public static GalaxyManager DeserializeGalaxyManager(string source){
		using (Stream s = GenerateStreamFromString(source))
		{
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.IgnoreWhitespace = true;
			XmlReader reader = XmlReader.Create(s, readerSettings);
			return DeserializeGalaxyManager(reader);
		}
	}

	[DeserializationMethod(typeof(Sector))]
	public static Sector DeserializeSector(XmlReader reader){
		GameObject sectorObj = new GameObject ();
		Sector sector = sectorObj.AddComponent<Sector>();
		while (reader.Read () && !(!reader.IsStartElement() && reader.LocalName.Equals(typeof(Sector).Name))) {
			if (reader.IsStartElement ()) {
				switch (reader.LocalName) {
				case "SECTOR_DATA":
					Sector.ReadXml (reader, sector);
					break;
				case "OBJECTS":
					PopulateSectorWithObjects (reader, sector);
					break;
				}
			}
		}
		// maybe we should unload default?
		// sector.Unload ();
		// @TODO: DECIDE HERE
		return sector;
	}

	private static void PopulateSectorWithObjects(XmlReader reader, Sector sector) {
		while (reader.Read () && !(!reader.IsStartElement () && reader.LocalName.Equals ("OBJECTS"))) {
			if (reader.IsStartElement ()) {
				Debug.Log (reader.LocalName);
				try{
					DType deserializer = deserializers [reader.LocalName];
					if(deserializer != null) {
						IUnityXmlSerializable deserializedThing = deserializer.Invoke(reader);
						if(deserializedThing){
							sector.AddToSector(deserializedThing.gameObject);
						}
					} else {
						Debug.LogError("Cannot find deserialization method for: " + reader.LocalName);
					}
				} catch (KeyNotFoundException k){
					Debug.LogError ("Cannot find Type Mapping for: " + reader.LocalName);
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

// @TODO: Method attribute reflection in static constructor // sorta done
// @TODO: Rejig type lookup to use System Type instead of hard-coded strings // sorta happened
// @TODO: Re-order some of the methods to give less headaches // lel
// @TODO: Documentation, hah... // HA
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;
using QventSystem;

public class GalaxyManager : IUnityXmlSerializable, QventHandler {

	// Instantiate things through this so they are managed by the galaxy!
	public static GalaxyManager instance;
	public int SectorSize;
	public int Width;
	public int Height;
	public float UpdateTime = 1;
	private float UpdateTimer = 0;
	public float CheckTime = 1;
	private float CheckTimer = 0;
	public Sector[,] Sectors; 	// [y][x] 
								// could be ragged i guess. if we allow null entries...
								// Doesn't make sense tbh, we can just have sectors that don't allow entry
	public bool Generate = true;

	[SerializeField]
	private Dictionary<GameObject, Vector2Int> Observers = new Dictionary<GameObject, Vector2Int>();
	public List<Sector> LoadedSectors;
	public int LoadRadius;
	public GenerationScheme Scheme;
	private bool initialized = false;

	void Start() {
		init ();
	}

	private void init() {
		if (initialized) {
			return;
		}

		if (instance != null) {
			return;
		}

		instance = this;
		Sectors = new Sector[Height, Width];

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				// spawn sectors.
				GameObject sectorGO = new GameObject("Sector " + x + ":" + y);
				Sector sector = sectorGO.AddComponent<Sector> ();
				Sectors [y,x] = sector;
				sector.index = new Vector2Int (x, y);
				sectorGO.transform.position = SectorToWorldPoint (new Vector2Int (x, y));
				sectorGO.transform.SetParent (this.transform);
			}
		}

		if (Generate) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					// spawn sectors.
					Sector sector = Sectors[y,x];
					Scheme.Generate (sector);
				}
			}
		}

		initialized = true;
	}

	void Update() {
		if (UpdateTimer <= 0) {
			LoadAreas ();
			UpdateTimer += UpdateTime;
		}
		if (CheckTimer <= 0) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					Sectors [y, x].Check ();
				}
			}

			List<GameObject> update = new List<GameObject> ();

			foreach (GameObject go in Observers.Keys) {
				UpdateLoadZone (Observers [go], WorldToSectorPoint (go.transform.position));
				update.Add (go);
			}

			foreach (GameObject go in update) {
				Observers [go] = WorldToSectorPoint (go.transform.position);
			}

			CheckTimer += CheckTime;
		}
		UpdateTimer -= Time.unscaledDeltaTime;
		CheckTimer -= Time.unscaledDeltaTime;
	}


	public void AddToSector(GameObject GO) {
		// right now we don't generate.
		try {
			Vector2Int sectorPos = WorldToSectorPoint(GO.transform.position);
			// @TODO add to the sector
			if(Sectors[sectorPos.y, sectorPos.x]){
				Sectors[sectorPos.y, sectorPos.x].AddToSector(GO);
			} else {
				Debug.LogWarning("Sector Does not Exist: " + sectorPos.x + " " + sectorPos.y);
			}
		} catch (IndexOutOfRangeException e) {
			
			Debug.LogWarning ("Out of Galaxy Bounds " + GO.name  + " " + e.Message);
		}
	}

	public void AddObserver(GameObject GO){
		if(!Observers.ContainsKey(GO)){
			Observers.Add (GO, WorldToSectorPoint(GO.transform.position));
			GO.GetComponent<Ship> ().RegisterListener (this);
		}
	}

	public void RemoveObserver(GameObject GO){
		GO.GetComponent<Ship> ().UnregisterListener (this);
		Observers.Remove (GO);
	}

	// probably run this at 1 second intervals
	public void LoadAreas() {
		foreach (GameObject observer in Observers.Keys) {
			if (!(WorldToSectorPoint (observer.transform.position).Equals (Observers [observer]))) {
				UpdateLoadZone(Observers[observer], WorldToSectorPoint(observer.transform.position));
			}
		}
	}

	public void UpdateLoadZone(Vector2Int prev, Vector2Int next) {

		for (int y = 0; y < LoadRadius * 2 + 1; y++) {
			for (int x = 0; x < LoadRadius * 2 + 1; x++) {
				if (prev.y - LoadRadius + y >= 0 && prev.y - LoadRadius + y < Height) {
					if (prev.x - LoadRadius + x >= 0 && prev.x - LoadRadius + x < Width) {
						Sectors [prev.y - LoadRadius + y, prev.x - LoadRadius + x].Unload ();
					}
				}
			}
		}


		for (int y = 0; y < LoadRadius * 2 + 1; y++) {
			for (int x = 0; x < LoadRadius * 2 + 1; x++) {
				if (next.y - LoadRadius + y >= 0 && next.y - LoadRadius + y < Height) {
					if (next.x - LoadRadius + x >= 0 && next.x - LoadRadius + x < Width) {
						Sectors [next.y - LoadRadius + y, next.x - LoadRadius + x].Load ();
					}
				}
			}
		}

		//		if (next.x > prev.x) {
//			// unload left side of area
//			for(int i = 0; i < LoadRadius * 2 + 1; i ++) {
//				Sector oldSector = Sectors [prev.y - LoadRadius + i, prev.x - LoadRadius];
//				Sector newSector = Sectors [next.y - LoadRadius + i, next.y + LoadRadius];
//				if (oldSector.Loaded) {
//					oldSector.Unload();
//				}
//				if (!newSector.Loaded) {
//					newSector.Load ();
//				}
//			}
//		} else if (next.x < prev.x){
//			// unload right side of area
//			for(int i = 0; i < LoadRadius * 2 + 1; i ++) {
//				Sector oldSector = Sectors [prev.y - LoadRadius + i, prev.x + LoadRadius];
//				Sector newSector = Sectors [next.y - LoadRadius + i, next.y - LoadRadius];
//				if (oldSector.Loaded) {
//					oldSector.Unload();
//				}
//				if (!newSector.Loaded) {
//					newSector.Load ();
//				}
//			}
//		}
//		if (next.y > prev.y) {
//			// unload bottom of area
//			for(int i = 0; i < LoadRadius * 2 + 1; i ++) {
//				Sector oldSector = Sectors [prev.y - LoadRadius, prev.x - LoadRadius + i];
//				Sector newSector = Sectors [next.y + LoadRadius, next.y - LoadRadius + i];
//				if (oldSector.Loaded) {
//					oldSector.Unload();
//				}
//				if (!newSector.Loaded) {
//					newSector.Load ();
//				}
//			}
//		} else if (next.y < prev.y) {
//			// unload top of area
//			for(int i = 0; i < LoadRadius * 2 + 1; i ++) {
//				Sector oldSector = Sectors [prev.y + LoadRadius, prev.x - LoadRadius + i];
//				Sector newSector = Sectors [next.y - LoadRadius, next.y - LoadRadius + i];
//				if (oldSector.Loaded) {
//					oldSector.Unload();
//				}
//				if (!newSector.Loaded) {
//					newSector.Load ();
//				}
//			}
//		}
	}

	// yuck duck
	// Unity 0,0 converted to exact center of sectors array
	public Vector2Int WorldToSectorPoint(Vector2 pos) {
		int xMid = Width  % 2 == 0 ? Width / 2 : (int) Mathf.Ceil((float)Width  / 2) - 1;
		int yMid = Height % 2 == 0 ? Width / 2 : (int) Mathf.Ceil((float)Height / 2) - 1;
		float x = Width  % 2 == 0 ? pos.x : pos.x + SectorSize/2;
		float y = Height % 2 == 0 ? pos.y : pos.y + SectorSize/2;
		int xPos = x < 0 ? (int) (x / SectorSize - 1) :(int) (x / SectorSize);
		int yPos = y < 0 ? (int) (y / SectorSize - 1) :(int) (y / SectorSize);
		return new Vector2Int (xPos + xMid, yPos + yMid);
	}

	public Vector2 SectorToWorldPoint(Vector2Int pos) {
		int xMid = Width % 2 == 0 ? Width/2 : (int) Mathf.Ceil((float)Width/2) - 1;
		int yMid = Height % 2 == 0 ? Width / 2 : (int)Mathf.Ceil ((float)Height / 2) - 1;
		int x = pos.x - xMid;
		int y = pos.y - yMid;
		float xpos = Width % 2 == 0 ? x * SectorSize + SectorSize/2 : x * SectorSize;
		float ypos = Height % 2 == 0 ? y * SectorSize + SectorSize/2 : y * SectorSize;
		return new Vector2 (xpos, ypos);
	}

	public Sector GetSector(Vector2Int Coordinates) {
		if (Coordinates.y >= Height) {
			throw new IndexOutOfRangeException ();
		}
		if (Coordinates.x >= Width) {
			throw new IndexOutOfRangeException ();
		}
		return Sectors [Coordinates.y, Coordinates.x];
	}

	public Sector GetSector(Vector2 position) {
		return GetSector (WorldToSectorPoint(position));
	}

	public Sector GetSector(GameObject ToGet) {
		return GetSector (ToGet.transform.position);
	}

	public static GameObject SpawnWorldObject(GameObject prefab, Vector3 position, Quaternion rotation = default(Quaternion)) {
		Sector sector = GalaxyManager.instance.GetSector (position);
		if (sector) {
			GameObject go = GameObject.Instantiate (prefab);
			go.transform.position = position;
			go.transform.rotation = rotation;
			sector.AddToSector(go);
			return go;
		} else {
			return null;
		}
	}

	public new static GameObject ReadXml(XmlReader reader, Component workingObj)
	{
		GalaxyManager galman = (GalaxyManager)workingObj;

		if (GalaxyManager.instance == galman) {
		} else {
			Debug.LogWarning ("More than one galaxy manager");
			return null;
		}

		reader.Read ();
		galman.SectorSize = int.Parse (reader.ReadString ());

		reader.Read ();
		galman.Width = int.Parse (reader.ReadString ());

		reader.Read ();
		galman.Height = int.Parse (reader.ReadString ());

		reader.Read ();
		galman.UpdateTime = float.Parse (reader.ReadString ());

		reader.Read ();
		galman.CheckTime = float.Parse (reader.ReadString ());
		return workingObj.gameObject;
	}

	public override void WriteXml (System.Xml.XmlWriter writer)
	{
		base.WriteXml (writer);
		writer.WriteStartElement ("GALAXY_DATA");

		writer.WriteElementString ("SECTOR_SIZE", SectorSize.ToString ());
		writer.WriteElementString ("WIDTH", Width.ToString ());
		writer.WriteElementString ("HEIGHT", Height.ToString ());
		writer.WriteElementString ("UPDATE_TIME", UpdateTime.ToString ());
		writer.WriteElementString ("CHECK_TIME", CheckTime.ToString ());

		writer.WriteEndElement ();

		writer.WriteStartElement ("SECTORS");

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				Sector sector = Sectors [y, x];
				if (sector) {
					SpaceSerializerDeserializer.MyMonoSerializeToStream (writer, sector);
				}
			}
		}

		writer.WriteEndElement ();
	}

	// warning! replaces old sector, by deleting it
	public void SetSector(Sector sector, Vector2Int index) {
		sector.gameObject.name = "Sector " + index.x + ":" + index.y;
		GameObject.Destroy (Sectors [index.y, index.x].gameObject);
		Sectors [index.y, index.x] = sector;
		sector.gameObject.transform.position = SectorToWorldPoint (index);
	}

	public void HandleQvent(Qvent q) {
		switch (q.QventType) {
		case QventType.DESTROYED:
			if (q.PayloadType.IsAssignableFrom(typeof(Ship))) {
				Observers.Remove (((Ship)q.Payload).gameObject);
			}
			break;
		}
	}
}

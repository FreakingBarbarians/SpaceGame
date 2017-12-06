using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSerialize : MonoBehaviour {
    public Ship shipToSerialize;
    public Module moduletoSerialize;
    public string path;
    public string path2;

    public string source;
    public bool run = false;

    public TextAsset shipFile;

    private void Update()
    {
        if (!run) {
            run = true;
            //SpaceSerializerDeserializer.SerializeShipToFile(shipToSerialize, path);
            //SpaceSerializerDeserializer.SerializeModuleToFile(moduletoSerialize, path2);
            //SpaceSerializerDeserializer.DeserializeModule(source);
            SpaceSerializerDeserializer.DeserializeShip(shipFile.text);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEditor;

public class PrefabGeneration : MonoBehaviour {
    [MenuItem("Assets/GenerateNamesAndPaths")]
    static void GenerateNamesAndPaths() {
        List<string> directories = new List<string>();
        string datapath = "Assets/Resources";
        GetAllDirectories(datapath, directories);
        foreach (string folder in directories) {
            
            string parsedFolder;
            if (folder.Equals("Assets/Resources")) {
                parsedFolder = "";
            }
            parsedFolder = folder.Substring("Assets/Resources".Length);
            Debug.Log(parsedFolder);
            GameObject[] objects = Resources.LoadAll<GameObject>(parsedFolder);
            foreach (GameObject go in objects)
            {
                MyPrefab[] monos = go.GetComponentsInChildren<MyPrefab>();
                string path = AssetDatabase.GetAssetPath(go);
                Debug.Log(path + "|" + go.name);
                foreach (MyPrefab m in monos)
                {
					if (m.transform.parent == null) {
						m.BASE_NAME = go.name;
						m.BASE_PATH = GetResourcesPath (path);
					}
                }
            }
        }
    }

    private static string GetResourcesPath(string path) {
        Regex rx = new Regex("Resources/.*");
        Match m = rx.Match(path);
        if (m.Success) {
            string p2 = m.ToString().Substring("Resources/".Length);
            rx = new Regex("[.].*");
            m = rx.Match(p2);
            if (m.Success) {
                return p2.ToString().Substring(0, p2.Length - m.ToString().Length);
            }
            return m.ToString().Substring("Resources/".Length);
        }
        return null;
    }

    private static List<string> GetAllDirectories(string path, List<string> objs) {
        path = path.Replace('\\', '/');
        objs.Add(path);
        foreach (string pt in Directory.GetDirectories(path)) {
            GetAllDirectories(pt, objs);
        }
        return objs;
    }
}

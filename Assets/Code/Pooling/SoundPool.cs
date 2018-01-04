using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour {

    public static SoundPool instance;
    public List<GameObject> ToPool;
    public Dictionary<string, UnityObjectPool<Sound>> mapping = new Dictionary<string, UnityObjectPool<Sound>>();
    public int poolSize = 100;

    void Start()
    {

        if (instance != null)
        {
            Debug.Log("Singleton violated for SoundPoolManager");
            Destroy(this);
        }

        instance = this;


        for (int i = 0; i < ToPool.Count; i++)
        {

            List<Sound> list = new List<Sound>();

            for (int x = 0; x < poolSize; x++)
            {
                GameObject go = Instantiate(ToPool[i]);
                list.Add(go.GetComponent<Sound>());
                go.SetActive(false);
                go.transform.SetParent(this.transform);
            }

            UnityObjectPool<Sound> pool = new UnityObjectPool<Sound>(list.ToArray());
            mapping.Add(ToPool[i].GetComponent<Sound>().Name, pool);
        }
    }

    public Sound Get(string id)
    {
        return mapping[id].Get();
    }

    public void Free(string id, Sound tofree)
    {
        mapping[id].Free(tofree);
    }

}

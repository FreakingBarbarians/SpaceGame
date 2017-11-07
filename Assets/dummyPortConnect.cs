using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyPortConnect : MonoBehaviour {

    public List<Port> toConnect;
    public List<Module> toBeConnected;

    bool ran = false;

    private void Update()
    {
        if (!ran) {
            run();            
        }
    }

    void run() {
        ran = true;
        for (int i = 0; i < toConnect.Count; i ++) {
            toConnect[i].Connect(toBeConnected[i]);
        }
    }
}

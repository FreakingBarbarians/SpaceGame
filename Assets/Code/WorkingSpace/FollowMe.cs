using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMe : MonoBehaviour {
    public GameObject target;

    public void Update()
    {
        target.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, target.transform.position.z);
    }

}

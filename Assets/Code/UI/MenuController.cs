using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    public static MenuController instance;

    void Start() {
        if (instance) {
            Debug.LogWarning("More than one menu controller instance");
            return;
        }
        instance = this;
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Enable()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

}

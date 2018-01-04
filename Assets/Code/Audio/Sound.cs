using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {
    public string Name;
    public AudioSource source;

    public void Play() {
        source.Play();
    }

    private void Update()
    {
        if (!source.isPlaying) {
            this.gameObject.SetActive(false);
            SoundPool.instance.Free(Name, this);
        }
    }

}

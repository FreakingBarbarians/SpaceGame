using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour {
    public static NotificationController instance;
    private List<string> queue = new List<string>();

    [Range(1,10)]
    public float DecayTime;
    private float decayTimer;

    public Text Text;

    private void Start()
    {
        if (instance) {
            Debug.LogWarning("More than one notification controller");
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (queue.Count > 0)
        {
            decayTimer -= Time.unscaledDeltaTime;
            if (decayTimer < 0)
            {
                queue.RemoveAt(0);
                decayTimer += DecayTime;
            }
        }
        else {
            decayTimer = DecayTime;
        }

        string s = "";
        for (int i = queue.Count - 1; i >= 0; i--) {
            s += queue[i] + "\n";
        }

        Text.text = s;
    }

    public void AddNotification(string noti) {
        queue.Add(noti);
    }

}

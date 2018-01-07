using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour {

    public float FadeInTime;
    private float fadeInTimer;
    public float HoldTime;
    private float holdTimer;
    private bool hold = false;

    public Text deathText;
    private Image im;


    private void Start()
    {
        im = GetComponent<Image>();
        fadeInTimer = FadeInTime;
        holdTimer = HoldTime;
    }

    public void Update()
    {
        float alphaVal = fadeInTimer / FadeInTime;

        Color c = im.color;
        c.a = 1 - alphaVal;
        im.color = c;

        c = deathText.color;
        c.a = 1 - alphaVal;
        deathText.color = c;

        if (fadeInTimer <= 0) {
            hold = true;
            if (holdTimer <= 0) {
                // @TODO: load main menu
                Debug.Log("LOAD MAIN MENU");
                SceneManager.LoadScene(0);
            }
            holdTimer -= Time.deltaTime;
        }
        fadeInTimer -= Time.deltaTime;
    }
}

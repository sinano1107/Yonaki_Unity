using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    float fadeSpeed = 0.02f;
    float red, green, blue, alfa;

    public bool isFadeOut = false;
    public bool isFadeIn = false;

    Image fadeImage;
    public Action action = () => {};

    void Start() {
        fadeImage = GameObject.Find("Panel").GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }

    void Update() {
        if (isFadeIn) StartFadeIn();
        if (isFadeOut) StartFadeOut();
    }

    void StartFadeIn() {
        alfa -= fadeSpeed;
        SetAlpha();
        if (alfa <= 0) {
            isFadeIn = false;
            fadeImage.enabled = false;
        }
    }

    void StartFadeOut() {
        fadeImage.enabled = true;
        alfa += fadeSpeed;
        SetAlpha();
        if (alfa >= 1) {
            isFadeOut = false;
            action();
            isFadeIn = true;
        }
    }

    void SetAlpha() {
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}

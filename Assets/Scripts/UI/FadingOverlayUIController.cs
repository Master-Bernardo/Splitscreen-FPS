using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingOverlayUIController : MonoBehaviour
{
    public Image overlayImage;
    public float fadeInSpeed;
    public float fadeOutSpeed;
    public float showDuration;
    float fadeOutStartTime;
    public float maxAlpha;

    Color defaultColor;

    enum State
    {
        Hidden,
        FadingIn,
        Shown,
        FadingOut,
    }

    State state;

    void Start()
    {
        defaultColor = overlayImage.color;
        overlayImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0); 
    }

    void Update()
    {
        switch (state)
        {
            case State.FadingIn:
                float alpha = overlayImage.color.a + fadeInSpeed * Time.deltaTime;

                if (alpha >= maxAlpha)
                {
                    state = State.Shown;
                    fadeOutStartTime = Time.time + showDuration;
                    overlayImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, maxAlpha);
                    break;
                }
                else
                {
                    overlayImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);

                }
                break;

            case State.Shown:

                if (Time.time > showDuration)
                {
                    state = State.FadingOut;
                }

                break;

            case State.FadingOut:

                alpha = overlayImage.color.a - fadeOutSpeed * Time.deltaTime;

                if (alpha <= 0)
                {
                    state = State.Hidden;
                    overlayImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
                    break;
                }
                else
                {
                    overlayImage.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);

                }
                break;

        }
    }

    public void ShowOverlay()
    {
        state = State.FadingIn;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlayUI : MonoBehaviour
{
    public Image damageOverlayImage;
    public float fadeInSpeed;
    public float fadeOutSpeed;
    public float showDuration;
    float fadeOutStartTime;
    public float maxAlpha;

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
        damageOverlayImage.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ShowDamageOverlay();
        }
        switch (state)
        {
            case State.FadingIn:
                float alpha = damageOverlayImage.color.a + fadeInSpeed * Time.deltaTime;

                if (alpha >= maxAlpha)
                {
                    state = State.Shown;
                    fadeOutStartTime = Time.time + showDuration;
                    damageOverlayImage.color = new Color(1, 1, 1, maxAlpha);
                    break;
                }
                else
                {
                    damageOverlayImage.color = new Color(1, 1, 1, alpha);

                }
                break;

            case State.Shown:

                if(Time.time> showDuration)
                {
                    state = State.FadingOut;
                }

                break;

            case State.FadingOut:

                alpha = damageOverlayImage.color.a - fadeOutSpeed * Time.deltaTime;

                if (alpha <= 0)
                {
                    state = State.Hidden;
                    damageOverlayImage.color = new Color(1, 1, 1, 0);
                    break;
                }
                else
                {
                    damageOverlayImage.color = new Color(1, 1, 1, alpha);

                }
                break;

        }
    }

    public void ShowDamageOverlay()
    {
        state = State.FadingIn;
    
    }
}

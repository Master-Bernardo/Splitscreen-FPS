using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashPointsUI : MonoBehaviour
{
    bool visible;
    bool dashPointsFullyLoaded;
    float nextDisableCanvasTime;
    [Tooltip("how long will this UI stay visible after the dash points are fully rechardged")]
    public float stayVisibleTime;
    public Canvas dashUICanvas;

    int maxDashPoints;
    int currentDashPoints;

    public GameObject[] dashPoints;
    public Image[] dashPointsFill;

    public void SetUp(int maxDashPoints)
    {
        for (int i = 0; i < dashPoints.Length; i++)
        {
            dashPoints[i].SetActive(false);
        }

        this.maxDashPoints = maxDashPoints;
        //set the UI
        for (int i = 0; i < maxDashPoints; i++)
        {
            dashPoints[i].SetActive(true);
            dashPointsFill[i].enabled = true;
        }
        SetInvisible();
    }

    void Update()
    {
        if (dashPointsFullyLoaded)
        {
            if(Time.time > nextDisableCanvasTime)
            {
                dashPointsFullyLoaded = false;
                SetInvisible();
            }
        }
        else if (visible)
        {
            if(currentDashPoints == maxDashPoints)
            {
                dashPointsFullyLoaded = true;
                nextDisableCanvasTime = Time.time + stayVisibleTime;
            }
        }

       
    }

    void SetVisible()
    {
        visible = true;
        dashUICanvas.enabled = true;
        dashPointsFullyLoaded = false;
    }

    void SetInvisible()
    {
        visible = false;
        dashUICanvas.enabled = false;
    }
    public void UpdateDashPoints(int currendDashPoints)
    {

        this.currentDashPoints = currendDashPoints;
        for (int i = 0; i < currendDashPoints; i++)
        {
            dashPointsFill[i].enabled = true;
        }
        for (int i = currendDashPoints; i < maxDashPoints; i++)
        {
            dashPointsFill[i].enabled = false ;
        }

        if (currendDashPoints < maxDashPoints) SetVisible();

    }
}

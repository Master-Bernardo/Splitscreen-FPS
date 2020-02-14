using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Text playerNumberText;
    public Text hordeModeHardnessText;
    public Text twoMonitorsText;


    void Start()
    {
        playerNumberText.text = GlobalSettings.playerNumber.ToString();
        hordeModeHardnessText.text = GlobalSettings.hordeModeHardness.ToString();
        twoMonitorsText.text = GlobalSettings.twoMonitors.ToString();
    }
  

    public void GoToHordeMode()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToVersusMode()
    {
        SceneManager.LoadScene(2);
    }

    public void RaisePlayerNumber()
    {
        GlobalSettings.playerNumber++;
        if (GlobalSettings.playerNumber > 4) GlobalSettings.playerNumber = 1;
        playerNumberText.text = GlobalSettings.playerNumber.ToString();
    }

    public void LowerPlayerNumber()
    {
        GlobalSettings.playerNumber--;
        if (GlobalSettings.playerNumber < 1) GlobalSettings.playerNumber = 4;
        playerNumberText.text = GlobalSettings.playerNumber.ToString();
    }

    public void RaiseHordeModeHardness()
    {
        GlobalSettings.hordeModeHardness += 0.2f;
        hordeModeHardnessText.text = GlobalSettings.hordeModeHardness.ToString();
    }

    public void LowerHordeModeHardness()
    {
        if (GlobalSettings.hordeModeHardness > 0.3f) GlobalSettings.hordeModeHardness -= 0.2f;
        
        hordeModeHardnessText.text = GlobalSettings.hordeModeHardness.ToString();
    }

    public void ToogleTwoMonitors()
    {
        GlobalSettings.twoMonitors = !GlobalSettings.twoMonitors;
        twoMonitorsText.text = GlobalSettings.twoMonitors.ToString();
    }
}

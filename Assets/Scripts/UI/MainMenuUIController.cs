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
    public Text enablePlayerMinionsInVersusText;
    public Text minionsMultiplierText;

    [Header("SubMenus")]
    public Canvas mainCanvas;
    public Canvas scenarioModeCanvas;
    public Canvas hordeModeCanvas;
    public Canvas versusModeCanvas;
    public Canvas controlsCanvas;


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerNumberText.text = GlobalSettings.playerNumber.ToString();
        hordeModeHardnessText.text = GlobalSettings.hordeModeHardness.ToString();
        twoMonitorsText.text = GlobalSettings.twoMonitors.ToString();
        minionsMultiplierText.text = GlobalSettings.versusUnitsAmount.ToString();
    }
  

    public void StartHordeMode()
    {
        SceneManager.LoadScene(3);
    }

    public void StartVersusMode()
    {
        SceneManager.LoadScene(2);
    }

    public void StartScenarioMode()
    {
        SceneManager.LoadScene(1);
    }

    public void GoBackToMainMenuPanel()
    {
        scenarioModeCanvas.enabled = false;
        hordeModeCanvas.enabled = false;
        versusModeCanvas.enabled = false;
        controlsCanvas.enabled = false;

        mainCanvas.enabled = true;
    }

    public void GoToScenarioModeSubmenu()
    {
        mainCanvas.enabled = false;
        scenarioModeCanvas.enabled = true;
    }

    public void GoToVersusModeSubmenu()
    {
        mainCanvas.enabled = false;
        versusModeCanvas.enabled = true;
    }

    public void GoToHordeModeSubmenu()
    {
        mainCanvas.enabled = false;
        hordeModeCanvas.enabled = true;
    }

    public void GoToControlsModeSubmenu()
    {
        mainCanvas.enabled = false;
        controlsCanvas.enabled = true;
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

    public void ToogleEnableMinionsInVersus()
    {
        GlobalSettings.enableAIInVersus = !GlobalSettings.enableAIInVersus;
        enablePlayerMinionsInVersusText.text = GlobalSettings.enableAIInVersus.ToString();
    }

    public void ToogleTwoMonitors()
    {
        GlobalSettings.twoMonitors = !GlobalSettings.twoMonitors;
        twoMonitorsText.text = GlobalSettings.twoMonitors.ToString();
    }

    public void RaiseMinionNumber()
    {
        GlobalSettings.versusUnitsAmount += 0.2f;
        minionsMultiplierText.text = GlobalSettings.versusUnitsAmount.ToString();
    }

    public void LowerMinionNumber()
    {
        GlobalSettings.versusUnitsAmount -= 0.2f;
        minionsMultiplierText.text = GlobalSettings.versusUnitsAmount.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CreditsUI : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject samPanel;
    [SerializeField] GameObject annaPanel;
    [SerializeField] GameObject nasimPanel;
    [SerializeField] GameObject colbyPanel;
    [SerializeField] GameObject tylerPanel;
    [SerializeField] GameObject rayPanel;


    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    [Header("List")]
    [SerializeField] private List<string> panelKeys = new List<string>();

    [Header("Int")]
    [SerializeField] private int currentPanelIndex = 0;

    void Start()
    {
        Debug.Log("CreditsUi is popping");
        panels.Add("SamPanel", samPanel);
        panels.Add("AnnaPanel", annaPanel);
        panels.Add("NasimPanel", nasimPanel);
        panels.Add("ColbyPanel", colbyPanel);
        panels.Add("TylerPanel", tylerPanel);
        panels.Add("RayPanel", rayPanel);

        panelKeys.Add("AnnaPanel");
        panelKeys.Add("ColbyPanel");
        panelKeys.Add("NasimPanel");
        panelKeys.Add("RayPanel");
        panelKeys.Add("SamPanel");
        panelKeys.Add("TylerPanel");

        SwitchPanel("AnnaPanel");
    }


    public void SwitchPanel(string panelName)
    {
        foreach (var panel in panels)
        {
            panel.Value.SetActive(panel.Key == panelName);
        }
    }

    public void NextPanel()
    {
        currentPanelIndex = (currentPanelIndex + 1) % panelKeys.Count;
        SwitchPanel(panelKeys[currentPanelIndex]);
    }

    public void PreviousPanel()
    {
        currentPanelIndex = (currentPanelIndex - 1) % panelKeys.Count;
        SwitchPanel(panelKeys[currentPanelIndex]);
    }
}
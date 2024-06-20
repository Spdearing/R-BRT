using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;

    public List<ResolutionItem> resolutions = new List<ResolutionItem>();
    private int selectedResolution;
    
    public TMP_Text resolutionLabel;

    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        bool foundResolution = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundResolution = true;

                selectedResolution = i;

                UpdateResolutionLabel();
            }
        }

        if(!foundResolution)
        {
            ResolutionItem newResolution = new ResolutionItem();
            newResolution.horizontal = Screen.width;
            newResolution.vertical = Screen.height;

            resolutions.Add(newResolution);
            selectedResolution = resolutions.Count - 1;

            UpdateResolutionLabel();
        }

    }

    
    void Update()
    {
        
    }

    public void ResolutionLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResolutionLabel();

    }

    public void ResolutionRight()
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResolutionLabel();

    }

    public void UpdateResolutionLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphicsChanges()
    {
        //Screen.fullScreen = fullscreenToggle.isOn;

        if(vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    }
}

[System.Serializable]
public class ResolutionItem
{
    public int horizontal, vertical;



}

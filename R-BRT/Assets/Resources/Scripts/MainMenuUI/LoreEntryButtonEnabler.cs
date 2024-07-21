using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreEntryButtonEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GameManager.instance.CheckLoreButtonStatus();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{





    public void LoadSurvivalSave()
    {
        // Load save data to file
        SurvivalData data = SaveSystem.LoadGame();
        
    }
}

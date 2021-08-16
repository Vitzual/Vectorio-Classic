using UnityEngine;
using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using System;
using System.Collections;

// Not displaying "no targets found" message correctly

public class Engineer : DefaultBuilding
{
    /*
    // Interface interaction
    private Interface UI;

    // Engineer variables
    public bool applyingModifications;
    public Transform engineerCog;

    // Internal placement variables
    public List<Transform> availableBuildings = new List<Transform>();
    
    // Called when awoken
    private void Start()
    {
        UI = GameObject.Find("Survival").GetComponent<Interface>();
    }

    // Update moves COG when applying a modification
    private void Update()
    {
        if (applyingModifications)
            engineerCog.Rotate(Vector3.forward, 50f * Time.deltaTime);
    }

    public IEnumerator StartEngineer(String building, int modID)
    {
        // Get the local time from the defense
        int localTime = 0;
        foreach (Transform turret in availableBuildings)
            if (turret.name == building)
            {
                break;
            }

        // Start the timer
        applyingModifications = true;
        //UI.OpenEngineer(true);
        do
        {
            UI.SetEngineerTimer(localTime + " SECONDS REMAINING...");
            yield return new WaitForSeconds(1);
            localTime--;
        }
        while (this != null && localTime > 0);
        if (this != null)
            FinishEngineer(building, modID);
    }

    public void FinishEngineer(string building, int modID)
    {
        // Stop spinning Cog
        applyingModifications = false;
        engineerCog.rotation = Quaternion.identity;

        // Refresh the UI
        UI.EngineerCooldownOverlay.SetActive(false);

        // Apply the modifications
        foreach (Transform turret in availableBuildings)
            if (turret.name == building && turret.GetComponent<TileClass>().IsModifiable())
                turret.GetComponent<TileClass>().ApplyModification(modID);
    }

    // Check for collectors
    public void CheckAdjacentTiles()
    {
        // Reset all child error transforms and set parsing between selected Engineer and the overlay
        foreach (Transform child in UI.EngineerList)
        {
            child.Find("Error").GetComponent<CanvasGroup>().alpha = 1;
            child.Find("Error").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        availableBuildings = new List<Transform>();
        var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name == "Turret")
            {
                if (colliders[i].GetComponent<TileClass>().IsModifiable())
                {
                    availableBuildings.Add(colliders[i].transform);
                    UI.EngineerList.Find(colliders[i].name).Find("Error").GetComponent<CanvasGroup>().alpha = 0;
                    UI.EngineerList.Find(colliders[i].name).Find("Error").GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }
    }
    */
}

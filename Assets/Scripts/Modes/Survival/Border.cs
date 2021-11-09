using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    // Border stage
    public enum Stage
    {
        Revenant,
        Kraken,
        Atlas,
        Serpent
    }
    public Stage borderStage;

    // Hub
    public Hub hub;

    // Borders
    public Transform[] borders;
    public int activatedBorder;
    public Vector2 newPosition;
    public float pushSpeed = 50f;

    // Border values
    public static int north = 750;
    public static int east = 750;
    public static int south = -750;
    public static int west = -750;

    // Border adjustment values
    public int borderIncrement = 750;

    // Grab event
    public void Start()
    {
        Events.active.fireHubLaser += NextStage;
    }

    // Activate push
    public void PushBorder()
    {
        borders[activatedBorder].position = Vector2.Lerp(borders[activatedBorder].position, newPosition, Time.deltaTime * pushSpeed);
    }

    // Progress stage
    public void NextStage()
    {
        if (hub == null) return;

        switch(borderStage)
        {
            // FIRST GUARDIAN
            case Stage.Revenant:
                borderStage = Stage.Kraken;
                north += borderIncrement;
                activatedBorder = 0;
                newPosition = new Vector2(0, borders[0].position.y + 750);
                hub.FireLaser(0);
                break;

            // SECOND GUARDIAN
            case Stage.Kraken:
                borderStage = Stage.Atlas;
                south -= borderIncrement;
                activatedBorder = 2;
                newPosition = new Vector2(0, borders[2].position.y - 750);
                hub.FireLaser(2);
                break;

            // THIRD GUARDIAN
            case Stage.Atlas:
                borderStage = Stage.Serpent;
                west -= borderIncrement;
                activatedBorder = 3;
                newPosition = new Vector2(borders[3].position.x - 750, 0);
                hub.FireLaser(3);
                break;

            // FOURTH GUARDIAN
            case Stage.Serpent:
                east += borderIncrement;
                activatedBorder = 1;
                newPosition = new Vector2(borders[1].position.x + 750, 0);
                hub.FireLaser(1);
                break;
        }
       
    }
}

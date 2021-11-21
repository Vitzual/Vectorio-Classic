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
    public int borderStartSize = 750;
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
    public int borderIncrement;

    // Active border instance
    public static Border active;

    public void Awake()
    {
        active = this;
    }

    // Grab event
    public void Start()
    {
        Events.active.fireHubLaser += NextStage;

        north = borderStartSize;
        east = borderStartSize;
        south = -borderStartSize;
        west = -borderStartSize;
    }

    // Set border
    public void SetBorder(Stage stage)
    {
        borderStage = stage;
        Stage holder = stage;

        if (holder == Stage.Serpent)
        {
            west -= borderIncrement;
            borders[3].position = new Vector2(borders[3].position.x + borderIncrement, 0);
            holder = Stage.Atlas;
        }
        if (holder == Stage.Atlas)
        {
            south -= borderIncrement;
            borders[2].position = new Vector2(0, borders[2].position.y - borderIncrement);
            holder = Stage.Revenant;
        }
        if (holder == Stage.Kraken)
        {
            north += borderIncrement;
            borders[0].position = new Vector2(0, borders[0].position.y + borderIncrement);
        }
    }

    // Activate push
    public void PushBorder()
    {
        borders[activatedBorder].position = Vector2.MoveTowards(borders[activatedBorder].position, newPosition, pushSpeed * Time.deltaTime);
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
                newPosition = new Vector2(0, borders[0].position.y + borderIncrement);
                hub.FireLaser(0);
                break;

            // SECOND GUARDIAN
            case Stage.Kraken:
                borderStage = Stage.Atlas;
                south -= borderIncrement;
                activatedBorder = 2;
                newPosition = new Vector2(0, borders[2].position.y - borderIncrement);
                hub.FireLaser(2);
                break;

            // THIRD GUARDIAN
            case Stage.Atlas:
                borderStage = Stage.Serpent;
                west -= borderIncrement;
                activatedBorder = 3;
                newPosition = new Vector2(borders[3].position.x - borderIncrement, 0);
                hub.FireLaser(3);
                break;

            // FOURTH GUARDIAN
            case Stage.Serpent:
                east += borderIncrement;
                activatedBorder = 1;
                newPosition = new Vector2(borders[1].position.x + borderIncrement, 0);
                hub.FireLaser(1);
                break;
        }
       
    }
}

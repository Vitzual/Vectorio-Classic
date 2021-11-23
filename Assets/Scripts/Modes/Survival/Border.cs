using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    // Borders
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    // Borders variables
    public Transform[] _borders;
    public static Transform[] borders;
    public int borderStartSize = 750;
    public static int borderIncrement = 350;

    // Border values
    public static int north = 750;
    public static int east = 750;
    public static int south = -750;
    public static int west = -750;

    // Colors
    public List<SpriteRenderer> borderColors;
    public List<SpriteRenderer> fillColors;

    // Grab event
    public void Start()
    {
        borders = _borders;

        north = borderStartSize;
        east = borderStartSize;
        south = -borderStartSize;
        west = -borderStartSize;

        SetBorderPositions();

        Events.active.onChangeBorderColor += UpdateBorderColor;
    }

    // Set stage
    public static void UpdateStage()
    {
        Stage stage = Gamemode.stage;
        while (stage.previousStage != null)
        {
            IncreaseBorderSize(stage.previousStage.guardianDirection);
            stage = stage.previousStage;
        }
        SetBorderPositions();
    }

    // Update border
    public void UpdateBorderColor(Material border, Color fill)
    {
        foreach (SpriteRenderer color in borderColors)
            color.material = border;
        foreach (SpriteRenderer color in fillColors)
            color.color = fill;
    }

    // Set border size
    public static void IncreaseBorderSize(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                north += borderIncrement;
                break;
            case Direction.East:
                east += borderIncrement;
                break;
            case Direction.South:
                south -= borderIncrement;
                break;
            case Direction.West:
                west -= borderIncrement;
                break;
        }
    }

    // Set border position
    public static void SetBorderPositions()
    {
        borders[(int)Direction.North].position = new Vector2(0, north);
        borders[(int)Direction.East].position = new Vector2(east, 0);
        borders[(int)Direction.South].position = new Vector2(0, south);
        borders[(int)Direction.West].position = new Vector2(west, 0);
    }

    // Activate push
    public static void PushBorder(Direction direction, float pushSpeed)
    {
        if (CheckBorderPosition(direction))
            borders[(int)direction].position = Vector2.MoveTowards(borders[(int)direction].position, GetBorderPosition(direction), pushSpeed * Time.deltaTime);
        else Debug.Log("Border passed size check");
    }

    // Check border posotion
    public static bool CheckBorderPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return borders[(int)Direction.North].position.y < north;
            case Direction.East:
                return borders[(int)Direction.East].position.x < east;
            case Direction.South:
                return borders[(int)Direction.South].position.y > south;
            case Direction.West:
                return borders[(int)Direction.West].position.x > west;
            default:
                Debug.Log("Invalid direction passed!");
                return false;
        }
    }

    // Get position
    public static Vector2 GetBorderPosition(Direction direction)
    {
        switch(direction)
        {
            case Direction.North:
                return new Vector2(0, north);
            case Direction.East:
                return new Vector2(east, 0);
            case Direction.South:
                return new Vector2(0, south);
            case Direction.West:
                return new Vector2(west, 0);
            default:
                Debug.Log("Invalid direction passed!");
                return Vector2.zero;
        }
    }
}

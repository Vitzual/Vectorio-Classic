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

    // Border values
    public static int north = 750;
    public static int east = 750;
    public static int south = 750;
    public static int west = 750;

    // Colors
    public List<SpriteRenderer> frontColors;
    public List<SpriteRenderer> backColors;

    // Grab event
    public void Start()
    {
        borders = _borders;

        north = borderStartSize;
        borders[(int)Direction.North].position = new Vector2(0, north);

        east = borderStartSize;
        borders[(int)Direction.East].position = new Vector2(east, 0);

        south = -borderStartSize;
        borders[(int)Direction.South].position = new Vector2(0, -south);

        west = -borderStartSize;
        borders[(int)Direction.West].position = new Vector2(-west, 0);
    }

    // Update border
    public void UpdateBorderColor(Variant variant)
    {
        foreach (SpriteRenderer color in frontColors)
            color.material = variant.border;
        foreach (SpriteRenderer color in backColors)
            color.material = variant.fill;
    }

    // Activate push
    public static void PushBorder(Direction direction, float pushSpeed)
    {
        borders[(int)direction].position = Vector2.MoveTowards(borders[(int)direction].position, GetBorderPosition(direction), pushSpeed * Time.deltaTime);
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
                return new Vector2(0, -south);
            case Direction.West:
                return new Vector2(-west, 0);
            default:
                Debug.Log("Invalid direction passed!");
                return Vector2.zero;
        }
    }
}

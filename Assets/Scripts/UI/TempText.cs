using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Ported from private Automa repository

public class TempText
{
    // Temp text variables
    public Transform transform;
    public TextMeshPro textMesh;
    private float time, speed, speedDecrease, alphaDecrease, alphaTime;
    [HideInInspector] public bool isActive = false;
    private Color c; // Allocate memory to color reference

    /// <summary>
    /// Constructor method (calls Set)
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    /// <param name="alphaDecrease"></param>
    /// <param name="alphaTime"></param>
    /// <param name="speedDecrease"></param>
    public TempText(TextMeshPro textMesh, Vector2 position, string text, Color color, float alphaDecrease = 2f,
        float alphaTime = 0.5f, float speedDecrease = 2f)
    {
        this.textMesh = textMesh;
        transform = textMesh.transform;
        Set(text, position, color, alphaDecrease, alphaTime, speedDecrease);
    }

    /// <summary>
    /// Sets up the temp text component
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    /// <param name="alphaDecrease"></param>
    /// <param name="alphaTime"></param>
    /// <param name="speedDecrease"></param>
    public void Set(string text, Vector2 position, Color color, float alphaDecrease = 2f,
        float alphaTime = 0.5f, float speedDecrease = 2f)
    {
        // Reset stats
        time = 1f;
        speed = 2f;
        isActive = true;
        transform.position = position;

        // Set animation variables
        this.alphaDecrease = alphaDecrease;
        this.alphaTime = alphaTime;
        this.speedDecrease = speedDecrease;

        // Set text color
        textMesh.text = text;
        textMesh.color = color;
    }

    /// <summary>
    /// Moves the temp text component
    /// </summary>
    public bool Update()
    {
        // Move the object up
        transform.position += transform.up * speed * Time.deltaTime;
        speed -= speedDecrease * Time.deltaTime;
        if (speed <= 0) speed = 0;
        time -= Time.deltaTime;

        // Lower opacity after time
        if (time <= alphaTime)
        {
            c = textMesh.color;
            c = new Color(c.r, c.g, c.b, c.a - (alphaDecrease * Time.deltaTime));
            textMesh.color = c;
        }

        // Scale based on time
        return textMesh.color.a <= 0f || time <= 0f;
    }
}
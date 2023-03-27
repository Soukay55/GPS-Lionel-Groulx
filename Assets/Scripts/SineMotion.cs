using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMotion : MonoBehaviour
{
    private Vector2 floatY;

    // `
    public const float FloatStrength = 0.35f, facteurAccélér = 1.5f;

    private float valeurDébut;

    private void Awake()
    {
        valeurDébut = transform.position.y;
    }

    private void Update()
    {
        floatY = transform.position;
        floatY.y = valeurDébut + Mathf.Sin(Time.time * facteurAccélér % (2 * Mathf.PI)) * FloatStrength;
        transform.position = floatY;
    }
}
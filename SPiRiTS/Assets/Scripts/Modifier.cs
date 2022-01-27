using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier
{
    [SerializeField] private ElementType element = ElementType.None;
    [SerializeField] private float mod = 1f;


    public ElementType Element => element;

    public float Mod => mod;
}

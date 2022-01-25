using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : Item
{
    [SerializeField] private int attackBuff = 0;
    [SerializeField] private int defenseBuff = 0;
    [SerializeField] private List<Element> resistance = new List<Element>();
    [SerializeField] private float resistMultiplier = 1f;
}

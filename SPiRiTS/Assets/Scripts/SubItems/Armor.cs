using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    [SerializeField] private int baseDefense = 0;
    [SerializeField] private List<Element> weeknesses = new List<Element>();
    [SerializeField] private float weeknessMultiplier=1f;
    [SerializeField] private List<Element> resistances = new List<Element>();
    [SerializeField] private float resitMultiplier = 0f;
}

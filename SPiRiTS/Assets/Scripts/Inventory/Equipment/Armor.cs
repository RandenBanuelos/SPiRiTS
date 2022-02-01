using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Create New Armor")]
public class Armor : Item
{
    [SerializeField] int baseDefense = 0;

    [SerializeField] List<ElementType> weakList = new List<ElementType>();
    [SerializeField] List<float> weakMultipliers = new List<float>();

    [SerializeField] List<ElementType> resistList = new List<ElementType>();
    [SerializeField] List<float> resistMultipliers = new List<float>();


    private Dictionary<ElementType, float> weaknesses;
    private Dictionary<ElementType, float> resistances;


    // FUNCTIONS

    private void Awake()
    {
        weaknesses = new Dictionary<ElementType, float>();
        resistances = new Dictionary<ElementType, float>();

        for (int i = 0; i < weakList.Count; i++)
            weaknesses.Add(weakList[i], weakMultipliers[i]);

        for (int i = 0; i < resistList.Count; i++)
            resistances.Add(resistList[i], resistMultipliers[i]);
    }

    public int BaseDefense => baseDefense;

    public Dictionary<ElementType, float> Weaknesses => weaknesses;

    public Dictionary<ElementType, float> Resistances => resistances;
}

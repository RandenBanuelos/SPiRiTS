using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Items that are considered armor that can be used by both players and enemies
/// </summary>
[CreateAssetMenu(fileName = "Armor", menuName = "Items/Create New Armor")]
public class Armor : Item
{
    /// <summary>
    /// Base defense stat of the armor item
    /// </summary>
    [SerializeField] int baseDefense = 0;


    /// <summary>
    /// List of weaknesses for this armor item
    /// </summary>
    [SerializeField] List<ElementType> weakList = new List<ElementType>();

    /// <summary>
    /// List of multipliers for the weaknesses of this armor item
    /// </summary>
    [SerializeField] List<float> weakMultipliers = new List<float>();


    /// <summary>
    /// List of resistances for this armor item
    /// </summary>
    [SerializeField] List<ElementType> resistList = new List<ElementType>();

    /// <summary>
    /// List of multipliers for the resistances of this armor item
    /// </summary>
    [SerializeField] List<float> resistMultipliers = new List<float>();

    /// <summary>
    /// Stores weaknesses and their multipliers, with weakness as the key and the multiplier as its value
    /// </summary>
    private Dictionary<ElementType, float> weaknesses;

    /// <summary>
    /// Stores resistances and their multipliers, with resistance as the key and the multiplier as its value
    /// </summary>
    private Dictionary<ElementType, float> resistances;


    // FUNCTIONS
    private void Awake()
    {
        weaknesses = new Dictionary<ElementType, float>();
        resistances = new Dictionary<ElementType, float>();

        // Adds the data to the dictionary
        for (int i = 0; i < weakList.Count; i++)
            weaknesses.Add(weakList[i], weakMultipliers[i]);

        for (int i = 0; i < resistList.Count; i++)
            resistances.Add(resistList[i], resistMultipliers[i]);
    }

    public int BaseDefense => baseDefense;

    public Dictionary<ElementType, float> Weaknesses => weaknesses;

    public Dictionary<ElementType, float> Resistances => resistances;
}

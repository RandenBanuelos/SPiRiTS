using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int money = 0;
    [SerializeField] private Dictionary<Item, int> items = new Dictionary<Item, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item, int amount)
    {
        if (!items.ContainsKey(item))
        {
            items.Add(item, amount);
        }
        else
        {
            int temp = items[item];
            int newValue = temp + amount;
            items[item] = newValue;
        }
    }
}

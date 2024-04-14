using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Action<bool, List<Item>> OnInitialize;

    public List<Item> Items { get; private set; } = new List<Item>();

    public PlayerInventory()
    {
        OnInitialize += OnInitializeInventory;
    }

    public void OnInitializeInventory(bool saved, List<Item> items = null)
    {
        Slots.Instance?.OnReloadSlots?.Invoke(Items);
    }

    public void AddItem(Item item)
    {
        if (Items.Count >= 5) return;

        item.gameObject.SetActive(false);
        Items.Add(item);
        Slots.Instance.OnReloadSlots.Invoke(Items);
    }

    public void RemoveItem(int index)
    {
        try
        {
            Items.RemoveAt(index);
            Slots.Instance.OnReloadSlots.Invoke(Items);
        }
        catch(Exception e)
        {
            Debug.Log("No item in that slot");
        }
    }
}
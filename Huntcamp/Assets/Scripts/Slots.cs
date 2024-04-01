using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public static Slots Instance { get; private set; }

    public List<GameObject> _slots = new List<GameObject>();

    public Action<List<Item>> OnReloadSlots;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;

        OnReloadSlots += OnReload;
    }

    public void OnClickSlot(int slot)
    {
        Player.Instance._inventory.RemoveItem(slot);
    }

    private void OnReload(List<Item> items)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < items.Count && items[i] is Item item && item != null)
            {
                _slots[i].GetComponentsInChildren<Image>()[1].sprite = item.Image;
            }
            else
            {
                _slots[i].GetComponentsInChildren<Image>()[1].sprite = null;
            }
        }
    }
}

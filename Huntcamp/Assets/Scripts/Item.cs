using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name { get; private set; }
    public Sprite Image;

    private void Start()
    {
        Name = gameObject.name;
    }
}

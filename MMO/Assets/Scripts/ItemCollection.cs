using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Collection", menuName = "Inventory/Item Collection")]
public class ItemCollection : ScriptableObject
{
    public Item[] items = new Item[0];
}

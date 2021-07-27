using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType itemType;
    public int chanceMultiplier;

    public Item(ItemType itemType, int chanceMultiplier)
    {
        this.itemType = itemType;
        this.chanceMultiplier = chanceMultiplier;
    }
}

public enum ItemType
{
    Helmet,
    Medkit,
    Ore,
    Coin,
    Pistol
}




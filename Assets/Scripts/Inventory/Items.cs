using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Create new Item")]
[System.Serializable]
public class Items : ScriptableObject
{

    public int itemId;
    public string itemName;

    [TextArea(3,3)] public string itemDescription;

    public enum ItemType{
        Consumable,
        Equipment,
        Weapon,
        Material,
        Quest
        
    }

    public enum ItemRarity{
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public GameObject itemPrefab;
    public Texture itemIcon;

    public ItemType itemType;
    public ItemRarity itemRarity;
    public int maxItemStack;
    public float itemWeight;
    public int itemBaseValue;
}


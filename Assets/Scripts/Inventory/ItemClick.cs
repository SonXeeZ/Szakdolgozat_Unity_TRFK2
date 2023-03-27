using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour
{
    public Slot clickedSlot;
    public Color[] rarityColors;
    RawImage itemImage;

    private TextMeshProUGUI item_Name_TMP;
    private TextMeshProUGUI item_Rarity_TMP;
    private TextMeshProUGUI item_Weight_TMP;
    private TextMeshProUGUI item_Value_TMP;
    private TextMeshProUGUI item_Type_TMP;
    private TextMeshProUGUI item_Stack_TMP;
    private TextMeshProUGUI item_Description_TMP;


    private void OnEnable(){
        Setup();
    }

    private void Awake(){
        InitializeComponents();
        
    }

    private void InitializeComponents(){
        itemImage = transform.GetChild(1).GetComponent<RawImage>();
        item_Name_TMP = transform.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        item_Rarity_TMP = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        item_Weight_TMP = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        item_Value_TMP = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        item_Type_TMP = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        item_Stack_TMP = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        item_Description_TMP = transform.GetChild(9).GetComponent<TextMeshProUGUI>();
    }
    

    private void Setup(){
        
        
        item_Name_TMP.text = clickedSlot.itemInSlot.itemName;
        item_Weight_TMP.text = $"Weight: {clickedSlot.itemInSlot.itemWeight * clickedSlot.AmountInSlot} kg";
        item_Stack_TMP.text = $"Stack: {clickedSlot.AmountInSlot}/{clickedSlot.itemInSlot.maxItemStack}";
        item_Value_TMP.text = $"Amount: {clickedSlot.AmountInSlot * clickedSlot.itemInSlot.itemBaseValue}";
        item_Description_TMP.text = clickedSlot.itemInSlot.itemDescription;
        itemImage.texture = clickedSlot.itemInSlot.itemIcon;

        switch(clickedSlot.itemInSlot.itemRarity){
            case Items.ItemRarity.Common:
                item_Rarity_TMP.text = "Common";
                item_Rarity_TMP.color = rarityColors[0];
                break;
            case Items.ItemRarity.Uncommon:
                item_Rarity_TMP.text = "Uncommon";
                item_Rarity_TMP.color = rarityColors[1];
                break;
            case Items.ItemRarity.Rare:
                item_Rarity_TMP.text = "Rare";
                item_Rarity_TMP.color = rarityColors[2];
                break;
            case Items.ItemRarity.Epic:
                item_Rarity_TMP.text = "Epic";
                item_Rarity_TMP.color = rarityColors[3];
                break;
            case Items.ItemRarity.Legendary:
                item_Rarity_TMP.text = "Legendary";
                item_Rarity_TMP.color = rarityColors[4];
                break;
            case Items.ItemRarity.Mythic:
                item_Rarity_TMP.text = "Mythic";
                item_Rarity_TMP.color = rarityColors[5];
                break;
            default:
                item_Rarity_TMP.text = "Unknown";
                item_Rarity_TMP.color = rarityColors[0];
                break;
        }

        switch (clickedSlot.itemInSlot.itemType)
        {
            case Items.ItemType.Consumable:
                item_Type_TMP.text = "Consumable";
                break;
            case Items.ItemType.Equipment:
                item_Type_TMP.text = "Equipment";
                break;
            case Items.ItemType.Weapon:
                item_Type_TMP.text = "Weapon";
                break;
            case Items.ItemType.Material:
                item_Type_TMP.text = "Material";
                break;
            case Items.ItemType.Quest:
                item_Type_TMP.text = "Quest Item";
                break;

            }
    }
    
}

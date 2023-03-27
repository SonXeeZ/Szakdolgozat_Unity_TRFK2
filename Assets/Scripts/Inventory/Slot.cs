using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Items itemInSlot;
    public int AmountInSlot;

    RawImage itemIcon;
    TextMeshProUGUI txt_Amount;

    public void SetStats(){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(true);
        }

        itemIcon = GetComponentInChildren<RawImage>();
        txt_Amount = GetComponentInChildren<TextMeshProUGUI>();

        if(itemInSlot == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            return;
        }

        itemIcon.texture = itemInSlot.itemIcon;
        txt_Amount.text = $"{AmountInSlot}x";
    }
}

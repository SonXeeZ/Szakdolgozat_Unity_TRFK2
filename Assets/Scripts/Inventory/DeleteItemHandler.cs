using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItemHandler : MonoBehaviour
{
    private Slot slot;
    private Items item;

    private ItemClick itemClick;
    
    public void DeleteCurrentItemFromSlot(Slot slot, ItemClick itemClick){
        slot.itemInSlot = null;
        slot.AmountInSlot = 0;
        slot.SetStats();
        itemClick.clickedSlot = null;
        itemClick.gameObject.SetActive(false);
    }
}


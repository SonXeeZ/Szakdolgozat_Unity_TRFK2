using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InventorySystem : NetworkBehaviour
{
    [SerializeField] public Slot[] slots = new Slot[40];
    [SerializeField] GameObject InventoryUI;


    private void Awake(){
        enabled = true;
        InventoryUI = GameObject.Find("InventoryPanel");
        InventoryUI.SetActive(false);
    }

    /*private void OnNetworkSpawn(){
        if(IsOwner){
            enabled = true;
            InventoryUI = GameObject.Find("InventoryPanel");
            InventoryUI.SetActive(false);
        }
           
    }*/

    private void Update(){
        EnableInventoryUI();
    }
        
    private void Start(){
        if(IsOwner){
            AssignSlots();
        }
    }

    private void EnableInventoryUI(){
        if(!InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.E)){
            InventoryUI.SetActive(true);
        }
        else if (InventoryUI.activeInHierarchy && Input.GetKeyDown(KeyCode.E) ||Input.GetKeyDown(KeyCode.Escape)){
            InventoryUI.SetActive(false);
        }
    }

    private void AssignSlots(){
        for(int i = 0; i < slots.Length; i++){
            if(InventoryUI.transform.GetChild(i).GetComponent<Slot>() != null){
                slots[i] = InventoryUI.transform.GetChild(i).GetComponent<Slot>();
                if(slots[i].itemInSlot == null){
                    for(int j = 0; j < slots[i].transform.childCount; j++){
                        slots[i].transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    
    public void PickUpItem(ItemObject obj)
    {
        int emptySlotIndex = -1;
        int stackableSlotIndex = -1;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemInSlot == null)
            {
                if (emptySlotIndex == -1)
                {
                    emptySlotIndex = i;
                }
            }
            else if (slots[i].itemInSlot.itemId == obj.itemStats.itemId && slots[i].AmountInSlot < slots[i].itemInSlot.maxItemStack)
            {
                if (stackableSlotIndex == -1)
                {
                    stackableSlotIndex = i;
                }
            }
        }

        if (stackableSlotIndex != -1)
        {
            int spaceAvailableInSlot = slots[stackableSlotIndex].itemInSlot.maxItemStack - slots[stackableSlotIndex].AmountInSlot;
            int amountToAdd = Mathf.Min(spaceAvailableInSlot, obj.amount);
            slots[stackableSlotIndex].AmountInSlot += amountToAdd;
            obj.amount -= amountToAdd;

            if (obj.amount == 0)
            {
                Destroy(obj.gameObject);
                slots[stackableSlotIndex].SetStats();
                return;
            }
        }

        if (emptySlotIndex != -1)
        {
        slots[emptySlotIndex].itemInSlot = obj.itemStats;
        slots[emptySlotIndex].AmountInSlot = obj.amount;
        Destroy(obj.gameObject);
        slots[emptySlotIndex].SetStats();
        return;
        }
    }
    

/*
public void PickUpItem(ItemObject obj)
{
    bool itemAdded = false;

    // searching for existing items with the same itemID
    for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i].itemInSlot != null && slots[i].itemInSlot.itemId == obj.itemStats.itemId && slots[i].AmountInSlot < slots[i].itemInSlot.maxItemStack)
        {
            int amountToAdd = Mathf.Min(slots[i].itemInSlot.maxItemStack - slots[i].AmountInSlot, obj.amount);

            slots[i].AmountInSlot += amountToAdd;
            obj.amount -= amountToAdd;
            slots[i].SetStats();

            if (obj.amount == 0)
            {
                itemAdded = true;
                Destroy(obj.gameObject);
                break;
            }
        }
    }

    // if the item was not added to an existing stack, find an empty slot
    if (!itemAdded)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemInSlot == null)
            {
                slots[i].itemInSlot = obj.itemStats;
                slots[i].AmountInSlot = Mathf.Min(obj.amount, obj.itemStats.maxItemStack);
                obj.amount -= slots[i].AmountInSlot;
                slots[i].SetStats();

                if (obj.amount == 0)
                {
                    Destroy(obj.gameObject);
                }

                break;
            }
        }
    }

    // if there is still an amount left, recursively call the method to add to the next slot
    if (obj.amount > 0)
    {
        PickUpItem(obj);
    }
}
*/

    bool WillHitMaxStack(int index, int amount)
    {
        if (slots[index].itemInSlot.maxItemStack <= slots[index].AmountInSlot + amount)
            return true;
        else
            return false;
    }

    int NeededToFill(int index)
    {
        return slots[index].itemInSlot.maxItemStack - slots[index].AmountInSlot;
    }
   int RemainingAmount(int index, int amount)
    {
        return  (slots[index].AmountInSlot + amount)-slots[index].itemInSlot.maxItemStack;
    }
}

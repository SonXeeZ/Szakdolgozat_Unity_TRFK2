using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteraction : NetworkBehaviour
{
    Camera playerCamera;
    [SerializeField] LayerMask itemLayer;
    InventorySystem inventorySystem;
    GameObject HoveredItem;
    [SerializeField] TextMeshProUGUI txt_HoveredItem;

    void Awake(){
    }

    void Start(){
        if(IsOwner){
            playerCamera = GetComponentInChildren<Camera>();
            HoveredItem = GameObject.Find("ItemHoverText");
            txt_HoveredItem = HoveredItem.GetComponentInChildren<TextMeshProUGUI>();
            txt_HoveredItem.text = string.Empty;
            inventorySystem = GetComponent<InventorySystem>();
        }
    }

    void Update(){

        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if(!Physics.Raycast(ray, out hit, 1000, itemLayer)) {
            txt_HoveredItem.text = string.Empty;
            return;
        }

        if(Physics.Raycast(ray, out hit, 1000, itemLayer)){

            if(!hit.collider.GetComponent<ItemObject>()) return;
            

            txt_HoveredItem.text = $"Press 'F' to pick up {hit.collider.GetComponent<ItemObject>().amount}x {hit.collider.GetComponent<ItemObject>().itemStats.itemName}";
            Debug.Log("Text of Item :" + txt_HoveredItem.text);

            if(Input.GetKeyDown(KeyCode.F)){
               inventorySystem.PickUpItem(hit.collider.GetComponent<ItemObject>());
               Debug.Log("Item picked up. Item:" + hit.collider.GetComponent<ItemObject>().itemStats.itemName + " Amount:" + hit.collider.GetComponent<ItemObject>().amount);
            }
        }else{
            

            txt_HoveredItem.text = string.Empty;
            Debug.Log("No item in range.");
        }
    }
}

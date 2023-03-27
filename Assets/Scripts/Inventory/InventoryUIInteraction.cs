using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUIInteraction : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] GameObject UIItemClick;
    private bool isEnteredWithMouse;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerClick.GetComponent<Slot>().itemInSlot == null || UIItemClick.activeInHierarchy){
            return;
        }
 
        if(eventData.button == PointerEventData.InputButton.Middle){
            UIItemClick.GetComponent<DeleteItemHandler>().DeleteCurrentItemFromSlot(eventData.pointerClick.GetComponent<Slot>(), UIItemClick.GetComponent<ItemClick>());
            return;
        }
        UIItemClick.GetComponent<ItemClick>().clickedSlot = eventData.pointerClick.GetComponent<Slot>();
        UIItemClick.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIItemClick.GetComponent<ItemClick>().clickedSlot = null;
        UIItemClick.SetActive(false);
    }
}

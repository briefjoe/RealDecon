using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Image image;
    [SerializeField] Color selectedColor;
    [SerializeField] Color baseColor;

    int xPos;
    int yPos;
    bool hotBar;

    bool contaminated = false;

    void Awake()
    {
        Deselect();
    }

    public void InitSlot (int x, int y, bool inHotBar)
    {
        xPos = x;
        yPos = y;
        hotBar = inHotBar;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.SetParentAfterDrag(transform);
        }
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = baseColor;
    }

    public void UpdateItem(bool contam)
    {
        //for when item is added, check if it's contaminated.

        if (contam)
        {

        }

        //to get the slot's item, just get the 0th child
    }

    IEnumerator ContaminateInventory()
    {
        yield return null;
    }
}

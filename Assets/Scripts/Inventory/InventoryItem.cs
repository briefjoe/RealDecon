using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField] Image image;
    [SerializeField] TMP_Text countText;

    Transform parentAfterDrag;
    
    Item item;
    int count = 1;

    bool contaminated;

    public void InitItem(Item newItem, bool contaminated)
    {
        item = newItem;
        image.sprite = newItem.image;

        if (contaminated)
        {
            image.color = WorldManager.Instance.GetConColor();
            this.contaminated = contaminated;
        }

        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();

        countText.gameObject.SetActive(count > 1);
    }

    //drag and drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void SetParentAfterDrag(Transform pad)
    {
        parentAfterDrag = pad;
    }

    public int GetCount()
    {
        return count;
    }

    public void ChangeCount(int amount)
    {
        count += amount;
    }

    public Item GetItem()
    {
        return item;
    }

    public bool GetContaminated()
    {
        return contaminated;
    }

    public void SetContamianted() 
    {
        if (item.canContaminate)
        {
            contaminated = true;
        }
    }
}

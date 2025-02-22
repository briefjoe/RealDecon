using UnityEngine;
using UnityEngine.UI;

public class SynthButton : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Image image;

    public void SetItem(Item i)
    {
        item = i;
        image.sprite = item.image;
    }

    public Item GetItem()
    {
        return item;
    }

    public void CraftItem()
    {
        CraftingManager.Instance.Craft(item.id);
    }
}

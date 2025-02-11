using UnityEngine;

public class TestScript: MonoBehaviour 
{
    public InventoryController ic;
    public Item[] itemstopickup;

    public void PickupItem(int id)
    {
        ic.AddItem(itemstopickup[id], false);
    }
}

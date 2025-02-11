using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryScreen; 
    [SerializeField] GameObject inventoryItemPrefab; 
    [SerializeField] InventorySlot[] inventorySlots;
    
    bool inInventory;

    int selectedSlot = 0;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.E) && !inInventory) {

            //open inventory and set game to be in inventory

            inventoryScreen.SetActive(true);
            inInventory = true;

            //disable player/enemy actions (will be handled in respective scripts) and set time scale to zero-----------------------------------------------------------------------
            //probably just handle this in a single global variables script

        }else if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) && inInventory)
        {
            //hide inventory and set game to be not in inventory

            inventoryScreen.SetActive(false);
            inInventory = false;
        }

        //change selected slot

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 9)
            {
                ChangeSelectedSlot(number-1);
            }
        }
    }

    public bool AddItem(Item item, bool contaminated)
    {
        //check if inventory has item already
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if(itemInSlot != null)
                Debug.Log(itemInSlot.GetContaminated() + " " + contaminated);

            if (itemInSlot != null && itemInSlot.GetItem() == item && itemInSlot.GetCount() < itemInSlot.GetItem().maxStack && itemInSlot.GetContaminated() == contaminated)
            {
                //increase amount if item in slot
                itemInSlot.ChangeCount(1);
                itemInSlot.RefreshCount();
                return true;
            }
        }

        //find empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                //add item in slot
                SpawnItem(item, inventorySlots[i], contaminated);
                return true;
            }
        }

        return false;
    }

    public void SpawnItem(Item item, InventorySlot slot, bool contaminated)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();

        inventoryItem.InitItem(item, contaminated);
    }

    void ChangeSelectedSlot(int newValue)
    {
        inventorySlots[selectedSlot].Deselect();

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public Item GetSelectedItem(bool useUp)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInslot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInslot != null)
        {
            Item item = itemInslot.GetItem();
            if (useUp)
            {
                //if this is a consumable item
                itemInslot.ChangeCount(-1);
                if(itemInslot.GetCount() <= 0)
                {
                    Destroy(itemInslot.gameObject);
                }
            }
            return item;
        }

        return null;
    }
}

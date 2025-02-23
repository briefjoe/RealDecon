using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] Item[] starterItems;

    [Header("Screen")]
    [SerializeField] GameObject inventoryScreen; 
    [SerializeField] GameObject inventoryItemPrefab; 
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] int hotBarSize = 8; //number of hotbar slots. Also determines the width of the inventory 


    InventorySlot[] hotbarSlots;
    InventorySlot[,] inventoryScreenSlots;
    
    bool inInventory;

    int selectedSlot = 0;

    Dictionary<Item, int> inventoryItemCounts; //store total counts of items in inventory

    private void Start()
    {
        hotbarSlots = new InventorySlot[hotBarSize];
        inventoryScreenSlots = new InventorySlot[inventorySlots.Length / hotBarSize, hotBarSize];
        inventoryItemCounts = new Dictionary<Item, int>();

        int j = 0;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < hotBarSize)
            {
                //fill in hotbar slots array
                hotbarSlots[j] = inventorySlots[i];

                inventorySlots[i].InitSlot(i, 0, true);
            }
            else
            {
                //fill in inventory slots array
                int index = i - hotBarSize;
                int row = index / hotBarSize;
                int col = index % hotBarSize;

                inventoryScreenSlots[row, col] = inventorySlots[i];

                inventorySlots[i].InitSlot(row, col, false);
            }
        }

        ChangeSelectedSlot(0);

        //set starter items in inventory
        for (int i = 0; i < starterItems.Length; i++)
        {
            AddItem(starterItems[i], false, 10);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.E) && !inInventory)
            {

                //open inventory and set game to be in inventory

                inventoryScreen.SetActive(true);
                inInventory = true;
                Global.inMenu = true;


            }
            else if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) && inInventory)
            {
                //hide inventory and set game to be not in inventory

                inventoryScreen.SetActive(false);
                inInventory = false;
                Global.inMenu = false;
            }

            //change selected slot

            if (Input.inputString != null)
            {
                bool isNumber = int.TryParse(Input.inputString, out int number);
                if (isNumber && number > 0 && number < 9)
                {
                    ChangeSelectedSlot(number - 1);
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeSelectedSlot(selectedSlot - 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeSelectedSlot(selectedSlot + 1);
            }
        }
    }

    public int AddItem(Item item, bool contaminated, int numberToAdd)
    {
        int remaining = numberToAdd;
            //check if inventory has item already
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null && itemInSlot.GetItem() == item && itemInSlot.GetCount() < itemInSlot.GetItem().maxStack && itemInSlot.GetContaminated() == contaminated)
                {
                    int num = Mathf.Min(remaining, itemInSlot.GetItem().maxStack - itemInSlot.GetCount());

                    //increase amount if item in slot
                    itemInSlot.ChangeCount(num);
                    itemInSlot.RefreshCount();

                    //update dictionary
                    if (inventoryItemCounts.ContainsKey(item))
                    {
                        inventoryItemCounts[item] += num + 1;
                    }
                    else
                    {
                        inventoryItemCounts[item] = num + 1;
                    }

                    remaining -= num;

                    if (remaining == 0)
                    {
                        return 0;
                    }
                }
            }

            //find empty slot if item doesn't exist
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {

                int num = Mathf.Min(remaining, item.maxStack);

                //add item in slot
                SpawnItem(item, inventorySlots[i], contaminated, num); //adds one item
                remaining -= num;

                if (inventoryItemCounts.ContainsKey(item))
                {
                    inventoryItemCounts[item] += num;
                }
                else
                {
                    inventoryItemCounts[item] = num;
                }

                if (remaining == 0)
                {
                    return 0;
                }
            }
        }

        return remaining;
    }

    public void SpawnItem(Item item, InventorySlot slot, bool contaminated, int count)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();

        inventoryItem.InitItem(item, contaminated);

        slot.UpdateItem(contaminated);

        inventoryItem.ChangeCount(count-1);
        inventoryItem.RefreshCount();
    }

    public bool RemoveItems(Item item, bool contaminated, int numberToRemove)
    {
        int remaining = numberToRemove;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.GetItem() == item && itemInSlot.GetContaminated() == contaminated)
            {
                int itemCount = itemInSlot.GetCount();

                //Determinte how many to remove from the slot
                int removeFromSlot = Mathf.Min(itemCount, remaining);

                //remove the imem from the slot
                itemInSlot.ChangeCount(-removeFromSlot);
                itemInSlot.RefreshCount();

                //update total item count in the dictionary
                if (inventoryItemCounts.ContainsKey(item))
                {
                    inventoryItemCounts[item] -= removeFromSlot;

                    if (inventoryItemCounts[item] <= 0)
                    {
                        inventoryItemCounts.Remove(item);
                    }
                }

                remaining -= removeFromSlot;

                if (itemInSlot.GetCount() <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }

                if (remaining <= 0)
                {
                    break;
                }
            }
        }

        return remaining == 0;
    }

    void ChangeSelectedSlot(int newValue)
    {
        inventorySlots[selectedSlot].Deselect();

        if (newValue >= hotbarSlots.Length)
        {
            newValue = 0;
        }
        else if (newValue < 0)
        {
            newValue = hotbarSlots.Length - 1;
        }

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
                itemInslot.RefreshCount();
                if(itemInslot.GetCount() <= 0)
                {
                    Destroy(itemInslot.gameObject);
                }
            }
            return item;
        }

        return null;
    }
    
    public InventorySlot[] GetHotBarSlots()
    {
        return hotbarSlots;
    }

    public InventorySlot[,] GetInventoryScreenSlots()
    {
        return inventoryScreenSlots;
    }

    public bool HasItem(string itemID, int amount)
    {
        foreach(var entry in inventoryItemCounts)
        {
            //Debug.Log("check: " + itemID + " amount: " + amount + " key: " + entry.Key.id +  " value: " + entry.Value);
            if(entry.Key.id == itemID && entry.Value >= amount)
            {
                return true;
            }
        }

        return false;
    }
}

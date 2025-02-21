using System.Linq;
using Unity.VisualScripting;
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

    private void Start()
    {
        hotbarSlots = new InventorySlot[hotBarSize];
        inventoryScreenSlots = new InventorySlot[inventorySlots.Length / hotBarSize, hotBarSize];

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
            AddItem(starterItems[i], false);
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
                Time.timeScale = 0f;

                //disable player/enemy actions (will be handled in respective scripts) and set time scale to zero-----------------------------------------------------------------------
                //probably just handle this in a single global variables script

            }
            else if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) && inInventory)
            {
                //hide inventory and set game to be not in inventory

                inventoryScreen.SetActive(false);
                inInventory = false;
                Global.inMenu = false;
                Time.timeScale = 1.0f;
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

    public bool AddItem(Item item, bool contaminated)
    {
        //check if inventory has item already
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();

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

        slot.UpdateItem(contaminated);
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
}

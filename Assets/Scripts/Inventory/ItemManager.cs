using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [SerializeField] List<Item> itemList;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}

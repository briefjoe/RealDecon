using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemType type;
    public ActionType actionType;

    [Header("Only UI")]
    public bool stackable;
    public int maxStack;
    public bool deconInventory;

    [Header("Both")]
    public Sprite image;
    public bool breakable;
    public float maxDurability;
    public bool canContaminate;
    //maybe add a boolean for if it's rechargable when I add recharging powered tools

    [Header("Item Specific")]
    public PlacableObject placable;

    public enum ItemType
    {
        Placable,
        Tool,
        Weapon,
        Gun
    }

    public enum ActionType
    {
        Place,
        Dig,
        Stab,
        Shoot
    }

}

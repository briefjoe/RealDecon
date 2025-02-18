using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class WorldItem : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Conaminable contamable;

    bool contaminated;

    private void Start()
    {
        spriteRenderer.sprite = item.image;
    }

    public Item GetItem()
    {
        return item; 
    }

    public void PickUp()
    {

        if(item.canContaminate && contamable != null && contamable.GetMaxCon() <= contamable.GetContamLevel())
        {
            contaminated = true;
        }
    }

    public bool GetContaminated()
    {
        return contaminated;
    }
}

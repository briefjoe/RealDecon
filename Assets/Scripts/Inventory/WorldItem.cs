using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class WorldItem : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Conaminable contamable;
    [SerializeField] float outlineThickness = 0.05f;

    Material material;
    bool contaminated;

    private void Start()
    {
        spriteRenderer.sprite = item.image;
        material = spriteRenderer.material;

        RemoveHighlight();
    }

    public Item GetItem()
    {
        return item; 
    }

    public void AddHighlight()
    {
        material.SetFloat("_OutlineThickness", outlineThickness);
    }

    public void RemoveHighlight()
    {
        material.SetFloat("_OutlineThickness", 0f);
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

using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float outlineThickness = 0.05f;

    Material material;

    void Start()
    {
        material = spriteRenderer.material;

        RemoveHighlight();
    }

    public void AddHighlight()
    {
        material.SetFloat("_OutlineThickness", outlineThickness);
    }

    public void RemoveHighlight()
    {
        material.SetFloat("_OutlineThickness", 0f);
    }
}

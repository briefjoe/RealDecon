using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class WorldItem : MonoBehaviour
{
    [SerializeField] Item item;

    [Header("Arc Movement")]
    [SerializeField] float arcDuration;
    [SerializeField] float arcHeight;
    [SerializeField] float maxArcWidth;
    [SerializeField] float minArcWidth;
    [SerializeField] float maxTravHeight;
    [SerializeField] float minTravHeight;
    float rotationSpeed = 5f;
    
    public bool contaminated;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.image;
    }

    public void Init(Item item, bool contaminated)
    {
        this.item = item;

        SetContaminated(contaminated);

        PopupMotion();
    }

    public void SetContaminated(bool contaminated)
    {
        if (GetComponent<Conaminable>() != null && contaminated)
        {
            this.contaminated = contaminated;

            GetComponent<Conaminable>().Contaminate(GetComponent<Conaminable>().GetMaxCon());
        }
    }

    public Item GetItem()
    {
        return item; 
    }

    public void PickUp()
    {

        if(item.canContaminate && GetComponent<Conaminable>() != null && GetComponent<Conaminable>().GetMaxCon() <= GetComponent<Conaminable>().GetContamLevel())
        {
            contaminated = true;
        }
    }

    public bool GetContaminated()
    {
        return contaminated;
    }

    public void PopupMotion()
    {
        float height = Random.Range(minTravHeight, maxTravHeight);
        float width = Random.Range(minArcWidth, maxArcWidth);

        Vector2 StartPos = transform.position;
        Vector2 EndPos = new Vector2(transform.position.x + width, transform.position.y);

        StartCoroutine(ArcMotion(StartPos, EndPos));
    }

    IEnumerator ArcMotion(Vector2 startPos, Vector2 endPos)
    {
        float timePassed = 0f;

        Vector2 previousPosition = startPos;

        while (timePassed < arcDuration)
        {
            float t = timePassed / arcDuration;

            Vector2 currentPos = Vector2.Lerp(startPos, endPos, t);

            float arcOffset = arcHeight * Mathf.Sin(Mathf.PI * t);

            currentPos.y += arcOffset;

            transform.position = currentPos;

            Vector2 velocity = (currentPos - previousPosition) / Time.deltaTime;

            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            if (velocity.y > 0)
            {
                angle -= 90;
            }
            else
            {
                angle += 90;
            }

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            previousPosition = currentPos;

            timePassed += Time.deltaTime;

            yield return null;
        }

        transform.position = endPos;
    }
}

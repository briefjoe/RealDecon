using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Tilemaps;

public class Conaminable : MonoBehaviour
{
    [SerializeField] float maxCont = 10f;
    [SerializeField] bool startContaminated;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] bool worldContam = true;

    float targetCont = 0f;
    public float curCont = 0;
    Color contamColor = Color.white;

    bool contaminated = false;
    public float conSpeed = 1f;
    bool transitioning = false;

    private void Start()
    {
        if (startContaminated)
        {
            contaminated = true;
            curCont = maxCont;
            ContaminationColor();
        }
    }

    private void FixedUpdate()
    {
        if (worldContam)
        {
            WorldTile t = WorldManager.Instance.GetWorldTiles()[GetPos().x][GetPos().y];

            conSpeed = t.GetContStrength();

            //contaminate/decontaminate player
            if (t.GetPurified())
            {
                if (!t.GetTransitioning() && curCont > 0)
                {
                    Contaminate(-1 * conSpeed * Time.deltaTime);
                }
            }
            else
            {
                Contaminate(t.GetContStrength() * Time.deltaTime);
            }
        }
    }

    public void UpdateTargetCont(float amount)
    {
        StopAllCoroutines();

        targetCont = amount;

        if (targetCont < curCont) 
        {
            //decontaminate
            StartCoroutine(DecontaminateObject());
        }
        else if (targetCont > curCont)
        {
            //contaminate
            StartCoroutine(ContaminateObject());
        }
    }

    public void SetContSpeed(float speed)
    {
        conSpeed = speed;
    }

    public IEnumerator ContaminateObject()
    {
        contaminated = true;

        //the function to increase contamination on objects
        while (curCont < maxCont)
        {

            Debug.Log("Contaminating");
            Contaminate(conSpeed * Time.deltaTime);

            if (curCont >= maxCont)
            {
                curCont = maxCont;

                ContaminationColor();

                break;
            }

            yield return null;
        }

        Debug.Log("Done contaminating");
    }

    public IEnumerator DecontaminateObject()
    {

        //the function to increase contamination on objects
        while (curCont > targetCont)
        {
            Contaminate(-conSpeed * Time.deltaTime);

            if (curCont <= targetCont)
            {

                curCont = targetCont;

                ContaminationColor();

                break;
            }

            yield return null;
        }

        contaminated = false;
    }

    public void Contaminate(float amount)
    {
        curCont = Mathf.Clamp(curCont + amount, 0, maxCont);

        ContaminationColor();
    }

    void ContaminationColor()
    {
        contamColor = Color.Lerp(Color.white, WorldManager.Instance.GetConColor(), curCont / maxCont);

        foreach (SpriteRenderer s in spriteRenderers)
        {
            s.color = contamColor;
        }
    }

    public float GetContamLevel()
    {
        return curCont;
    }

    public float GetMaxCon()
    {
        return maxCont;
    }

    public Vector2Int GetPos()
    {
        return new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }

    public bool GetContaminated()
    {
        return contaminated;
    }
}

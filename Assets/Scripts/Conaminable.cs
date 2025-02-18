using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Tilemaps;

public class Conaminable : MonoBehaviour
{
    [SerializeField] float maxCont = 10f;
    [SerializeField] bool startContaminated;
    [SerializeField] float conSpeed = 0.1f;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] bool worldContam = true;

    float targetCont = 0f;
    float curCont = 0;
    Color contamColor = Color.white;

    bool contaminated = false;

    private void Start()
    {
        if (startContaminated)
        {
            curCont = maxCont;
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
        Debug.Log("Contaminating");

        contaminated = true;

        //the function to increase contamination on objects
        while (curCont < maxCont)
        {

            Contaminate(conSpeed * Time.deltaTime);
            Debug.Log("Con: " + curCont);

            if (curCont >= maxCont)
            {
                curCont = maxCont;

                ContaminationColor();

                break;
            }

            yield return null;
        }

        yield return null;
    }

    public IEnumerator DecontaminateObject()
    {
        Debug.Log("decontaminating");

        contaminated = false;

        //the function to increase contamination on objects
        while (curCont > targetCont)
        {
            Contaminate(-conSpeed * Time.deltaTime);
            Debug.Log("Decon: " + curCont);


            if (curCont <= targetCont)
            {

                curCont = targetCont;

                ContaminationColor();

                break;
            }

            yield return null;
        }

        yield return null;
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
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public bool GetContaminated()
    {
        return contaminated;
    }
}

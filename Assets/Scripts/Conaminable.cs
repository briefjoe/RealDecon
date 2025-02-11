using UnityEngine;

public class Conaminable : MonoBehaviour
{
    [SerializeField] float maxContamination = 10f;
    [SerializeField] bool startContaminated;
    [SerializeField] float conSpeed = 1f;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    float curCont = 0;
    Color contamColor = Color.white;

    private void Start()
    {
        if (startContaminated)
        {
            curCont = maxContamination;
        }
    }

    private void FixedUpdate()
    {
        WorldTile t = WorldManager.Instance.GetWorldTiles()[GetPos().x][GetPos().y];

        //contaminate/decontaminate player
        if (t.GetPurified())
        {
            if (!t.GetTransitioning())
            {
                Contaminate(-1 * conSpeed * Time.deltaTime);
            }
        }
        else
        {
            Contaminate(t.GetContStrength() * Time.deltaTime);
        }
    }

    public void Contaminate(float amount)
    {
        if (curCont < maxContamination)
        {
            curCont = Mathf.Clamp(curCont + amount, 0, maxContamination);

            ContaminationColor();
        }
    }

    void ContaminationColor()
    {
        contamColor = Color.Lerp(Color.white, WorldManager.Instance.GetConColor(), curCont / maxContamination);
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
        return maxContamination;
    }

    Vector2Int GetPos()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }
}

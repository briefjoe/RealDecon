using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ResourceNode : PlacableObject
{

    [Header("Resource Node")]
    [SerializeField] Item resource;
    [SerializeField] float regrowTimeMin;
    [SerializeField] float regrowTimeMax;
    [SerializeField] GameObject[] resourcePoints;

    bool[] resourceBools; //tracks which resource points are grown

    void Start()
    {
        //place the object
        Place(new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)));

        GetComponent<Conaminable>().Contaminate(GetComponent<Conaminable>().GetMaxCon());

        //set bools array
        resourceBools = new bool[resourcePoints.Length];
        for (int i = 0; i < resourcePoints.Length; i++)
        {
            resourceBools[i] = true;
        }

        //grow initial fruit?
        for (int i = 0; i < resourcePoints.Length; i++)
        {
            resourcePoints[i].GetComponent<SpriteRenderer>().sprite = resource.image;
        }
    }

    public override void DestroyObject(Item item)
    {
        if (item != null && item.actionType == breakType)
        {
            if (!GetComponent<Conaminable>().GetContaminated())
            {
                Harvest();
            }

            //maybe do something to track the hits the player has done to the node
        }
    }

    void Harvest()
    {
        //drop the items that are grown
        for (int i = 0; i < resourcePoints.Length; ++i)
        {
            if (resourceBools[i])
            {
                resourcePoints[i].SetActive(false);
                resourceBools[i] = false;

                WorldItem tmp = Instantiate(WorldManager.Instance.GetWorldItemPrefab(), resourcePoints[i].transform.position, Quaternion.identity);
                tmp.Init(resource, GetComponent<Conaminable>().GetContaminated());
            }
        }

        //start regrowing the items
        for (int i = 0; i < resourcePoints.Length; i++)
        { 

            //how do i not restart the coroutines for already regrowing fruits?

            //could make a list of fruits that are currently regrowing, then remove the indexes of them once they're grown----------------------------------------------------------------------

            if (!resourceBools[i])
            {
                StartCoroutine(RegrowItems(i));
            }
        }
    }

    void GrowFruit(int point)
    {
        resourcePoints[point].SetActive(true);
        resourceBools[point] = true;
    }

    IEnumerator RegrowItems(int point)
    {
        //take time to regrow the fruit
        yield return new WaitForSeconds(Random.Range(regrowTimeMin, regrowTimeMax));

        //regrow fruit
        GrowFruit(point);
    }
}

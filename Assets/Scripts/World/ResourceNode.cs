using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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
        Place(new Vector2Int((int)transform.position.x, (int)transform.position.y));

        //set bools array
        resourceBools = new bool[resourcePoints.Length];

        //update contamination

        //grow initial fruit?
    }

    public override void DestroyObject()
    {
        Harvest();

        //maybe do something to track the hits the player has done to the node
    }

    void Harvest()
    {
        Debug.Log("Harvest");

        //drop the items that are grown


        //start regrowing the items
        for (int i = 0; i < resourcePoints.Length; i++)
        {
            StartCoroutine(RegrowItems(i));
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

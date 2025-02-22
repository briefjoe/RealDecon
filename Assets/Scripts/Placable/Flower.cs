using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Flower : PlacableObject
{
    [SerializeField] Sprite flowerShape;
    [SerializeField] float convSpeed; //how fast the flower converts tiles

    int clearSize;

    public override void Place(Vector2Int placePos)
    {
        base.Place(placePos);

        clearSize = flowerShape.texture.height;

        StartCoroutine(Decontaminate());
    }

    public float GetConvSpeed()
    {
        return convSpeed;
    }

    IEnumerator Decontaminate()
    {
        //go through the tiles in the range and start edecontaminating them in the WorldTileController
        for(int i = -Mathf.FloorToInt(clearSize / 2.0f); i <= Mathf.FloorToInt(clearSize / 2.0f); i++)
        {
            for (int j = -Mathf.FloorToInt(clearSize / 2.0f); j <= Mathf.FloorToInt(clearSize / 2.0f); j++) {
                int posX = i + Mathf.FloorToInt(clearSize / 2.0f);
                int posY = j + Mathf.FloorToInt(clearSize / 2.0f);

                if (flowerShape.texture.GetPixel(posX, posY).a != 0 && flowerShape.texture.GetPixel(posX, posY).r >= WorldManager.Instance.GetWorldTiles()[x + i][y + j].GetContStrength())
                {
                    //add flower to tile's active flowers
                    WorldManager.Instance.GetWorldTiles()[x + i][y + j].AddFlower(this); //this should be enough to trigger the decontamination to start
                }
            }
        }

        yield return null; 
    }

    public override void DestroyObject(Item item)
    {
        StartCoroutine(Contaminate(item));
    }

    IEnumerator Contaminate(Item item)
    {
        //go through the tiles in the range and start decontaminating them in the WorldTileController
        for (int i = -Mathf.FloorToInt(clearSize / 2.0f); i <= Mathf.FloorToInt(clearSize / 2.0f); i++)
        {
            for (int j = -Mathf.FloorToInt(clearSize / 2.0f); j <= Mathf.FloorToInt(clearSize / 2.0f); j++)
            {
                int posX = i + Mathf.FloorToInt(clearSize / 2.0f);
                int posY = j + Mathf.FloorToInt(clearSize / 2.0f);

                if (flowerShape.texture.GetPixel(posX, posY).a != 0 && flowerShape.texture.GetPixel(posX, posY).r >= WorldManager.Instance.GetWorldTiles()[x + i][y+j].GetContStrength())
                {
                    //remove flower from tile's active flowers
                    WorldManager.Instance.GetWorldTiles()[x + i][y + j].RemoveFlower(this); //this should end up causing the contamination to start.
                }
            }
        }
        yield return null;

        WorldManager.Instance.GetWorldTiles()[x][y].DestroyObject();
        WorldItem tmp = Instantiate(WorldManager.Instance.GetWorldItemPrefab(), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);

        tmp.Init(dropItem, false);

        Destroy(gameObject);
    }

    public Sprite GetFlowerShape()
    {
        return flowerShape;
    }

    public float GetConAtTile(Vector2Int pos)
    {
        return flowerShape.texture.GetPixel(pos.x, pos.y).r;
    }
}

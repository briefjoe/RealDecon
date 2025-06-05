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

        StartCoroutine(Transition(true));
    }

    public float GetConvSpeed()
    {
        return convSpeed;
    }

    public override void DestroyObject(Item item)
    {
        StartCoroutine(Transition(false, item));
    }

    IEnumerator Transition(bool decontaminating, Item item = null)
    {

        int halfSize = Mathf.FloorToInt(clearSize/2);
        WorldTile[][] tiles = WorldManager.Instance.GetWorldTiles();

        for (int i = -halfSize; i <= halfSize; i++)
        {
            for (int j = -halfSize; j <= halfSize; j++)
            {
                int posX = i + halfSize;
                int posY = j + halfSize;

                if (IsPixelActive(posX, posY) && PixelStrongerThanTile(posX, posY, tiles[x + i][y + j]))
                {
                    if (decontaminating)
                    {
                        //add flower to the tile's active flowers
                        tiles[x + i][y + j].AddFlower(this);
                    }
                    else
                    {
                        //remove flower from tile's active flowers
                        tiles[x + i][y + j].RemoveFlower(this); //this should end up causing the contamination to start.
                    }
                }
            }
        }



        if (!decontaminating && dropItem != null)
        {
            yield return null;

            WorldManager.Instance.GetWorldTiles()[x][y].DestroyObject();
            WorldItem tmp = Instantiate(WorldManager.Instance.GetWorldItemPrefab(), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);

            tmp.Init(dropItem, false);

            Destroy(gameObject);
        }
        else
        {
            yield return null;
        }
    }

    public Sprite GetFlowerShape()
    {
        return flowerShape;
    }

    public float GetConAtTile(Vector2Int pos)
    {
        return flowerShape.texture.GetPixel(pos.x, pos.y).r;
    }

    bool IsPixelActive(int x, int y)
    {
        return flowerShape.texture.GetPixel(x, y).a != 0;
    }

    bool PixelStrongerThanTile(int x, int y, WorldTile tile)
    {
        return flowerShape.texture.GetPixel(x, y).r >= tile.GetContStrength();
    }
}

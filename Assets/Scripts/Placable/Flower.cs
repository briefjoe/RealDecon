using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Flower : PlacableObject
{
    [SerializeField] Sprite flowerShape;
    [SerializeField] float convSpeed; //how fast the flower converts tiles

    int clearSize;

    public override void Place(WorldManager worldManager, int xPos, int yPos)
    {
        base.Place(worldManager, xPos, yPos);

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
                if (flowerShape.texture.GetPixel(i + Mathf.FloorToInt(clearSize / 2.0f), j + Mathf.FloorToInt(clearSize / 2.0f)).a != 0)
                {

                    //add flower to tile's active flowers
                    world.GetWorldTiles()[x + i][y + j].AddFlower(this);

                    //start decontaminating tiles
                    world.GetTileController().StartCoroutine(world.GetTileController().Decontaminate(world.GetWorldTiles()[x + i][y + j], this, flowerShape.texture.GetPixel(i + clearSize / 2, j + clearSize / 2).r));
                }
            }
        }

        yield return null; 
    }

    public override void Destroy()
    {
        base.Destroy();

        StartCoroutine(Contaminate());
    }

    IEnumerator Contaminate()
    {
        for (int i = -Mathf.FloorToInt(clearSize / 2.0f); i <= Mathf.FloorToInt(clearSize / 2.0f); i++)
        {
            for (int j = -Mathf.FloorToInt(clearSize / 2.0f); j <= Mathf.FloorToInt(clearSize / 2.0f); j++)
            {
                if (flowerShape.texture.GetPixel(i + Mathf.FloorToInt(clearSize / 2.0f), j + Mathf.FloorToInt(clearSize / 2.0f)).a != 0)
                {
                    //remove flower from tile's active flowers
                    world.GetWorldTiles()[x + i][y + j].RemoveFlower(this);

                    //start decontaminating tiles
                    world.GetTileController().StartCoroutine(world.GetTileController().Contaminate(world.GetWorldTiles()[x + i][y + j]));
                }
            }
        }
        yield return null;
    }
}

using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    [SerializeField] WorldItem worldItem;
    [SerializeField] Vector2Int baseSize;
    [SerializeField] SpriteRenderer sprite;

    protected int x;
    protected int y;

    public void PreviewObject(Vector2Int hoverPos)
    {
        //move object to hover position
        transform.position = new Vector2(hoverPos.x + 0.5f, hoverPos.y + 0.5f);

        //change color depending on if object can be placed
        if (CheckCanPlace(hoverPos))
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            sprite.color = new Color(1, 0, 0, 0.5f);
        }
    }

    public virtual void Place(Vector2Int placePos)
    {
        //called when the object is placed

        x = placePos.x;
        y = placePos.y;

        sprite.color = new Color(1, 1, 1, 1);

        //set all the tiles underneath it to be ocupied
        for (int i = 0; i < baseSize.x; i++)
        {
            for (int j = 0; j < baseSize.y; j++)
            {
                WorldManager.Instance.GetWorldTiles()[x+i][y+j].PlaceObject(this);
            }
        }

        transform.position = new Vector3(x +0.5f, y + 0.5f, 0);
    }

    public virtual bool CheckCanPlace(Vector2Int placePos)
    {
        for (int i = 0; i < baseSize.x; i++)
        {
            for (int j = 0; j < baseSize.y; j++)
            {

                //if tile is occupied, return false
                if (WorldManager.Instance.GetWorldTiles()[placePos.x + i][placePos.y + j].GetHasObject())
                {
                    return false;
                }
            }
        }

        return true;
    }

    public virtual void DestroyObject()
    {
        //called when the object is destroyed

        //clear object from all tiles it's on
        for(int i = 0;i < baseSize.x; i++)
        {
            for(int j = 0; j < baseSize.y; j++)
            {
                WorldManager.Instance.GetWorldTiles()[x + i][y+j].DestroyObject();
            }
        }

        //spawn item in world
        if (worldItem != null)
        {
            Instantiate(worldItem, transform.position, Quaternion.identity);
        }

        //clear world tile of object
        Destroy(gameObject);
    }

    public Vector2Int GetPos()
    { 
        return new Vector2Int(x, y);
    }
}

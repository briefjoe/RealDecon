using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    [SerializeField] WorldItem worldItem;
    [SerializeField] Vector2Int baseSize;

    protected int x;
    protected int y;

    public virtual void Place(int xPos, int yPos)
    {
        //called when the object is placed

        //default: set world tile at coordinates to be occupied by this.

        x = xPos;
        y = yPos;

        transform.position = new Vector3(x +0.5f, y + 0.5f, 0);

        WorldManager.Instance.GetWorldTiles()[x][y].PlaceObject(this);
    }

    public virtual bool CheckCanPlace()
    {
        for (int i = 0; i < baseSize.x; i++)
        {
            for (int j = 0; j < baseSize.y; j++)
            {

                //if tile is occupied, return false
                if (WorldManager.Instance.GetWorldTiles()[i][j].GetHasObject())
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

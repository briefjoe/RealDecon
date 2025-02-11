using JetBrains.Annotations;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    [SerializeField] WorldItem worldItem;

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

    public virtual void DestroyObject()
    {
        //called when the object is destroyed

        //spawn item in world
        Instantiate(worldItem, transform.position, Quaternion.identity);

        //clear world tile of object
        Destroy(gameObject);
    }

    public Vector2Int GetPos()
    { 
        return new Vector2Int(x, y);
    }
}

using JetBrains.Annotations;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    protected WorldManager world;

    protected int x;
    protected int y;

    public virtual void Place(WorldManager worldManager, int xPos, int yPos)
    {
        //called when the object is placed

        //default: set world tile at coordinates to be occupied by this.

        world = worldManager;
        x = xPos;
        y = yPos;

        transform.position = new Vector3(x +0.5f, y + 0.5f, 0);

        world.GetWorldTiles()[x][y].PlaceObject(this);
    }

    public virtual void Destroy()
    {
        //called when the object is destroyed

        //spawn item in world

        //clear world tile of object
        Destroy(gameObject);
    }
}

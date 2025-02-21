using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    [SerializeField] WorldItem worldItem;
    [SerializeField] Vector2Int baseSize;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] BoxCollider2D baseCollider; //the collideable area of the base

    [Header("Selection")]
    [SerializeField] BoxCollider2D interactCollider; //the trigger area for the interaction
    [SerializeField] bool selectable; //if the player can select the object

    protected int x;
    protected int y;

    WorldTile[,] placedTiles;

    public void PreviewObject(Vector2Int hoverPos)
    {
        //move object to hover position
        transform.position = new Vector2(hoverPos.x + 0.5f, hoverPos.y + 0.5f);

        //disable colliders
        if (baseCollider != null)
        {
            baseCollider.enabled = false;
        }

        if(interactCollider != null)
        {
            interactCollider.enabled = false; 
        }

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

        //reenable colliders
        if (baseCollider != null)
        {
            baseCollider.enabled = true;
        }

        if (interactCollider != null)
        {
            interactCollider.enabled = true;
        }

        placedTiles = new WorldTile[baseSize.x,baseSize.y];

        //set all the tiles underneath it to be ocupied
        for (int i = 0; i < baseSize.x; i++)
        {
            for (int j = 0; j < baseSize.y; j++)
            {
                WorldManager.Instance.GetWorldTiles()[x+i][y+j].PlaceObject(this);
                placedTiles[i, j] = WorldManager.Instance.GetWorldTiles()[x + i][y + j];
            }
        }

        transform.position = new Vector3(x +0.5f, y + 0.5f, 0);

        UpdateContamination();
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

        Debug.Log("Destroying");

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

    public void UpdateContamination()
    {
        float maxCont = 0f;

        if (GetComponent<Conaminable>() != null)
        {
            int deconCount = 0;

            for (int i = 0; i < baseSize.x; i++)
            {
                for (int j = 0; j < baseSize.y; j++)
                {
                    if (placedTiles[i, j].GetPurified())
                    {
                        //if tile is decontaminated
                        deconCount++;
                    }
                    else
                    {
                        if(maxCont < placedTiles[i, j].GetContStrength())
                        {
                            maxCont = placedTiles[i, j].GetContStrength();
                        }
                    }
                }
            }

            if (deconCount > 0 && GetComponent<Conaminable>().GetContaminated())
            {
                //decontaminate
                GetComponent<Conaminable>().UpdateTargetCont(0);
            }
            else if(deconCount == 0 && !GetComponent<Conaminable>().GetContaminated())
            {
                //contaminate
                GetComponent<Conaminable>().SetContSpeed(maxCont);
                GetComponent<Conaminable>().UpdateTargetCont(10);
            }
        }
    }

    public virtual void Interact() 
    {
        //what happens when the player interacts with the object

        Debug.Log("Clicked");
    }

    public bool GetSelectable()
    {
        return selectable;
    }
}

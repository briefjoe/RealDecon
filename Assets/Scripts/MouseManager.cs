using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Tilemap worldMap;
    [SerializeField] GameObject hoverObject;
    [SerializeField] InventoryController inventoryController;
    [SerializeField] LayerMask worldItemMask;

    WorldItem lastHover;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int tilePos = worldMap.WorldToCell(mouseWorldPos);

        TileBase hoveredTile = worldMap.GetTile(tilePos);
            
        hoverObject.SetActive(true);

        if(lastHover != null)
        {
            lastHover.RemoveHighlight();
            lastHover = null;

        }

        //check if hovering over item
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100, worldItemMask);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<WorldItem>() != null)
        {
            WorldItem tmp = hit.collider.gameObject.GetComponent<WorldItem>();

            //hilight the selected item
            tmp.AddHighlight();
            lastHover = tmp;

            hoverObject.SetActive(false);

            //pick up the item if clicked
            if (Input.GetMouseButtonDown(0))
            {
                tmp.PickUp(); //set item to be contaminated
                if (inventoryController.AddItem(tmp.GetItem(), tmp.GetContaminated()))
                {
                    Destroy(tmp.gameObject);
                }
            }
        }
        else
        {
            //if not hovering over item, select tile
            if (hoveredTile != null)
            {
                hoverObject.transform.position = new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                //player uses item in hand
                Item item = inventoryController.GetSelectedItem(false);

                if (item != null)
                {
                    if (item.actionType == Item.ActionType.Place)
                    {
                        //place placeable
                        PlacableObject po = Instantiate(item.placable, tilePos, Quaternion.identity);
                        po.Place(tilePos.x, tilePos.y);
                        inventoryController.GetSelectedItem(true);
                    }
                    else if (item.actionType == Item.ActionType.Dig)
                    {
                        //destroy selected object in world
                        if (WorldManager.Instance.GetWorldTiles()[tilePos.x][tilePos.y].GetHasObject())
                        {
                            WorldManager.Instance.GetWorldTiles()[tilePos.x][tilePos.y].DestroyObject();

                        }
                    }
                    else if (item.actionType == Item.ActionType.Stab)
                    {
                        //swing weapon
                    }
                    else if (item.actionType == Item.ActionType.Shoot)
                    {
                        //shoot gun
                    }

                    //player.DestroyObject(tilePos.x, tilePos.y);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //player interacts with object in the world

                //player.Interact(tilePos.x, tilePos.y);
            }
        }
    }
}

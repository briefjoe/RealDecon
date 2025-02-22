using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class MouseManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Tilemap worldMap;
    [SerializeField] GameObject hoverObject;
    [SerializeField] InventoryController inventoryController;
    [SerializeField] Item stoneItem;
    [SerializeField] LayerMask worldItemMask;
    [SerializeField] LayerMask placedObjectMask;

    HighlightObject lastHover;
    PlacableObject previewObject;

    // Update is called once per frame
    void Update()
    {
        if (!Global.inMenu && !Global.isPaused)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePos = worldMap.WorldToCell(mouseWorldPos);

            TileBase hoveredTile = worldMap.GetTile(tilePos);

            hoverObject.SetActive(true);

            if (lastHover != null)
            {
                lastHover.RemoveHighlight();
                lastHover = null;

            }

            //check if hovering over item
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100, worldItemMask);
            //check if hovering over a placed object
            RaycastHit2D hit2 = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100, placedObjectMask);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<WorldItem>() != null)
            {
                WorldItem tmp = hit.collider.gameObject.GetComponent<WorldItem>();

                //hilight the selected item
                tmp.GetComponent<HighlightObject>().AddHighlight();
                lastHover = tmp.GetComponent<HighlightObject>();

                hoverObject.SetActive(false);

                //pick up the item if clicked
                if (Input.GetMouseButtonDown(0))
                {
                    tmp.PickUp(); //set item to be contaminated
                    if (inventoryController.AddItem(tmp.GetItem(), tmp.GetContaminated(), 1) == 0)
                    {
                        Destroy(tmp.gameObject);
                    }
                }
            }
            //if hovering over a selectable object
            else if (hit2.collider != null && hit2.collider.gameObject.GetComponent<PlacableObject>() != null && hit2.collider.gameObject.GetComponent<PlacableObject>().GetSelectable())
            {
                PlacableObject tmp = hit2.collider.gameObject.GetComponent<PlacableObject>();

                tmp.GetComponent<HighlightObject>().AddHighlight();
                lastHover = tmp.GetComponent<HighlightObject>();

                hoverObject.SetActive(false);

                Item item = inventoryController.GetSelectedItem(false);

                if (Input.GetMouseButtonDown(1))
                {
                    //interact with placed object
                    tmp.Interact();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    //destroy the placed object
                    tmp.DestroyObject(item);
                }
            }
            else
            {
                //item preview
                Item item = inventoryController.GetSelectedItem(false);

                bool canPlace = false;

                if (item != null && item.placable != null)
                {
                    if (previewObject != null && PrefabUtility.GetCorrespondingObjectFromSource(previewObject) != item.placable)
                    {
                        //remove item preview if player isn't holding the same item as they had before
                        Destroy(previewObject.gameObject);
                        previewObject = null;
                    }

                    //create preview if there is no preview for current item
                    if (previewObject == null)
                    {
                        previewObject = Instantiate(item.placable, new Vector3(tilePos.x, tilePos.y, 0), Quaternion.identity);
                    }

                    canPlace = item.placable.CheckCanPlace(new Vector2Int(tilePos.x, tilePos.y));
                }
                else
                {
                    if (previewObject != null)
                    {

                        Destroy(previewObject.gameObject);
                    }
                }

                //if not hovering over item, select tile
                if (hoveredTile != null)
                {
                    hoverObject.transform.position = new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);

                    if (previewObject != null)
                    {
                        previewObject.PreviewObject(new Vector2Int(tilePos.x, tilePos.y));
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    //player uses item in hand

                    if (item != null)
                    {
                        if (item.actionType == Item.ActionType.Place)
                        {
                            if (canPlace)
                            {
                                //place placeable
                                PlacePlaceable(item, new Vector2Int(tilePos.x, tilePos.y));
                            }
                            else
                            {
                                //Debug.Log("Area blocked");
                            }
                        }
                        else if (item.actionType == Item.ActionType.Dig)
                        {
                            if (WorldManager.Instance.GetWorldTiles()[tilePos.x][tilePos.y].GetHasObject())
                            {
                                //destroy selected object in world
                                DestoryPlaceable(new Vector2Int(tilePos.x, tilePos.y), item);
                            }
                            else
                            {
                                //dig up stone
                                WorldItem tmp = Instantiate(WorldManager.Instance.GetWorldItemPrefab(), new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, 0), Quaternion.identity);
                                tmp.Init(stoneItem, !WorldManager.Instance.GetWorldTiles()[tilePos.x][tilePos.y].GetPurified());
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
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    //player interacts with object in the world
                }
            }

            //flip player sprite
            if (tilePos.x - player.GetComponent<Conaminable>().GetPos().x > 0)
            {
                player.GetPlayerSprite().transform.localScale = new Vector2(-1,1);
            }
            else if (tilePos.x - player.GetComponent<Conaminable>().GetPos().x < 0)
            {
                player.GetPlayerSprite().transform.localScale = new Vector2(1,1);
            }
        }
    }

    public void PlacePlaceable(Item item, Vector2Int tilePos)
    {
        //PlacableObject po = Instantiate(item.placable, new Vector3(tilePos.x, tilePos.y, 0), Quaternion.identity);
        previewObject.Place(tilePos);
        inventoryController.GetSelectedItem(true);
        previewObject = null;
    }

    public void DestoryPlaceable(Vector2Int tilePos, Item item)
    {
        if (WorldManager.Instance.GetWorldTiles()[tilePos.x][tilePos.y].GetHasObject())
        {
            WorldManager.Instance.GetWorldTiles()[tilePos.x][tilePos.y].GetPlacedObject().DestroyObject(item);

        }
    }
}

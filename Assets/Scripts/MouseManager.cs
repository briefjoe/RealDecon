using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Tilemap worldMap;
    [SerializeField] GameObject hoverObject;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3Int tilePos = worldMap.WorldToCell(mouseWorldPos);

        TileBase hoveredTile = worldMap.GetTile(tilePos);

        if(hoveredTile != null)
        {
            hoverObject.transform.position = new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //player uses item in hand
            player.DestroyObject(tilePos.x, tilePos.y);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //player interacts with item in the world
            player.Interact(tilePos.x, tilePos.y);
        }
    }
}

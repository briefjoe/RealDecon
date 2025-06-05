using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTileController : MonoBehaviour
{

    [SerializeField] PlayerController player;

    //move this to somewhere else once I do different regions
    [SerializeField] float worldContSpeed = 0.1f;

    public IEnumerator Decontaminate(WorldTile tile)
    {
        yield return null;

        tile.BeginDecon();

        while (tile.GetTransitioning() && tile.GetContLevel() > tile.GetTargetCont())
        {
            tile.ChangeContLevel(-worldContSpeed * Time.deltaTime);

            //update color based on conversion progress
            UpdateTileColor(tile);
            yield return null;

            if (tile.GetContLevel() * 100 <= tile.GetTargetCont() * 100)
            {
                tile.EndDecon();

                tile.SetContLevel(tile.GetContLevel());

                UpdateTileColor(tile);

                break;
            }
        }
    }

    public IEnumerator Contaminate(WorldTile tile)
    {
        yield return null;
        tile.BeginCon();

        while (tile.GetTransitioning() && tile.GetContLevel() < tile.GetTargetCont())
        {
            //Debug.Log("Contaminating");
            tile.ChangeContLevel(worldContSpeed * Time.deltaTime);

            //update color based on conversion progress
            UpdateTileColor(tile);
            yield   return null;

            if(tile.GetContLevel() >= tile.GetTargetCont())
            {
                tile.EndCon();

                tile.SetContLevel(tile.GetContLevel());

                UpdateTileColor(tile);

                break;
            }
        }
    }


    void UpdateTileColor(WorldTile tile)
    {
        Vector3Int tilePos = new Vector3Int(tile.GetX(), tile.GetY(), 0);
        Tilemap map = WorldManager.Instance.GetWorldMap();

        map.SetTileFlags(tilePos, TileFlags.None);
        map.SetColor(tilePos, Color.Lerp(WorldManager.Instance.GetDeconColor(), WorldManager.Instance.GetConColor(), tile.GetContLevel()));
    }
}

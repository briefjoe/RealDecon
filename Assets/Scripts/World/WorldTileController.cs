using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Unity.Burst.Intrinsics.Arm;
using UnityEngine.WSA;

public class WorldTileController : MonoBehaviour
{
    [SerializeField] WorldManager worldManager;

    [SerializeField] PlayerController player;

    //move this to somewhere else once I do different regions
    [SerializeField] float worldContSpeed = 0.1f;
    [SerializeField] float playerDeconSpeed = 8.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        WorldTile t = worldManager.GetWorldTiles()[player.GetPos().x][player.GetPos().y];

        //contaminate/decontaminate player
        if (t.GetPurified())
        {
            if (!t.GetTransitioning())
            {
                player.ContaminatePlayer(-1 * playerDeconSpeed * Time.deltaTime);
            }
        }
        else
        {
            player.ContaminatePlayer(t.GetContStrength() * Time.deltaTime);
        }
    }

    public IEnumerator Decontaminate(WorldTile tile)
    {
        yield return null;

        tile.BeginDecon();

        while (tile.GetTransitioning() && tile.GetContLevel() > tile.GetTargetCont())
        {
            tile.ChangeContLevel(-worldContSpeed * Time.deltaTime);

            //update color based on conversion progress
            worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), TileFlags.None);
            worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));
            yield return null;

            if (tile.GetContLevel() * 100 <= tile.GetTargetCont() * 100)
            {
                tile.EndDecon();

                tile.SetContLevel(tile.GetContLevel());

                worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), TileFlags.None);
                worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));

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
            worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), TileFlags.None);
            worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));
            yield return null;

            if(tile.GetContLevel() >= tile.GetTargetCont())
            {
                tile.EndCon();

                tile.SetContLevel(tile.GetContLevel());

                worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), TileFlags.None);
                worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));

                break;
            }
        }
    }

    /*
    public IEnumerator Decontaminate(WorldTile tile, Flower f, float deconLevel)
    {
        if (tile.GetContaminating())
        {
            tile.EndCon();
        }

        //set target contamination level

        //start decontaminate coroutine on the tile at the position

        //check if tile is already decontaminated stronger than the flower will decontaminate it
        if (!tile.GetDecontaminating() && tile.GetContLevel() > deconLevel)
        {
            tile.BeginDecon();
            //set all the appropriate flags for the current tile

            //Decontaminate
            while (tile.GetDecontaminating() && tile.GetContLevel() > deconLevel)
            {
                tile.ChangeContLevel(-f.GetConvSpeed() * Time.deltaTime);

                //update color based on conversion progress
                worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), UnityEngine.Tilemaps.TileFlags.None);
                worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));

                yield return null;
            }

            tile.SetContLevel(deconLevel);

            tile.EndDecon();
        }

        yield return null;
    }

    public IEnumerator Contaminate(WorldTile tile)
    {

        //check if already transitioning
        if (!tile.GetDecontaminating() || tile.GetActiveFlowers().Count == 0)
        {
            //start contaminate routine on the tile at the position

            //CHECK FOR DECON LEVEL OF MAIN FLOWER IF THERE IS ONE.------------------------
            float conLevel = tile.GetContStrength();

            //if too decontaminated
            if (tile.GetContLevel() < conLevel)
            {

            }

            tile.BeginCon();

            while (tile.GetContaminating() && tile.GetContLevel() < conLevel)
            {
                tile.ChangeContLevel(worldContSpeed * Time.deltaTime);

                //update color based on conversion progress
                worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), UnityEngine.Tilemaps.TileFlags.None);
                worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));

                yield return null;
            }

            tile.SetContLevel(conLevel);

            tile.EndCon();
        }

        yield return null; 
    
    }
    */
}

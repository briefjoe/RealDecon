using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

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
            player.ContaminatePlayer(-1 * playerDeconSpeed * Time.deltaTime);
        }
        else
        {
            player.ContaminatePlayer(t.GetContStrength() * Time.deltaTime);
        }
    }

    public IEnumerator Decontaminate(WorldTile tile, Flower f, float deconLevel)
    {
        //start decontaminate coroutine on the tile at the position

        //check if tile is already decontaminated stronger than the flower will decontaminate it
        if (!tile.GetConverting() && tile.GetContLevel() > deconLevel)
        {
           //set all the appropriate flags for the current tile
            tile.BeginDecon();

            //Debug.Log("Begin Contaminating");

            //Decontaminate
            while (tile.GetConverting() && tile.GetContLevel() > deconLevel)
            {
                tile.ChangeContLevel(-f.GetConvSpeed() * Time.deltaTime);

                //Debug.Log(tile.GetContLevel());

                //update color based on conversion progress
                worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), UnityEngine.Tilemaps.TileFlags.None);

                worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));

                yield return null;
            }

            tile.SetContLevel(deconLevel);

            tile.EndDecon();

            //Debug.Log("Done Decontaminating");
        }

        yield return null;
    }

    public IEnumerator Contaminate(WorldTile tile)
    {
        //start contaminate routine on the tile at the position

        //CHECK FOR DECON LEVEL OF ACTIVE FLOWER IF THERE IS ONE. THIS WILL BE FOR IF THERE IS NO ACTIVE FLOWER. 
        float conLevel = tile.GetContStrength();

        //if too decontaminated
        if (tile.GetContLevel() < conLevel)
        {

        }

        //THIS IS FOR WHEN THERE IS NO ACTIVE FLOWER

        tile.BeginCon();

        while (tile.GetConverting() && tile.GetContLevel() < conLevel)
        {
            tile.ChangeContLevel(worldContSpeed * Time.deltaTime);

            //update color based on conversion progress
            worldManager.GetWorldMap().SetTileFlags(new Vector3Int(tile.GetX(), tile.GetY(), 0), UnityEngine.Tilemaps.TileFlags.None);

            worldManager.GetWorldMap().SetColor(new Vector3Int(tile.GetX(), tile.GetY(), 0), Color.Lerp(worldManager.GetDeconColor(), worldManager.GetConColor(), tile.GetContLevel()));

            yield return null;
        }

        tile.SetContLevel(conLevel);

        tile.EndCon();


        yield return null; 
    
    }
}

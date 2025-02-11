using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [Header("Scene")]
    [SerializeField] Tilemap worldMap; //stores the ground tiles
    [SerializeField] Tilemap waterMap; //stores the water tiles
    [SerializeField] PlayerController player;
    [SerializeField] MouseManager mouseManager;
    [SerializeField] WorldTileController tileController;

    [Header("Map Sprites")]
    [SerializeField] Sprite waterImage;
    [SerializeField] Sprite mapImage;
    [SerializeField] Sprite worldTileSprite;

    [SerializeField] Color conColor;
    [SerializeField] Color deconColor;
    

    int width;
    int height;

    WorldTile[][] tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;

        //set map size based on map sprite size
        width = mapImage.texture.height;
        height = mapImage.texture.width;

        Tile t = ScriptableObject.CreateInstance<Tile>();
        t.sprite = worldTileSprite;

        tiles = new WorldTile[width][];
        for(int i = 0; i < width; i++)
        {
            tiles[i] = new WorldTile[height];
            for(int j = 0; j < height; j++)
            {

                //set tiles
                if (mapImage.texture.GetPixel(i, j).a > 0)
                {
                    worldMap.SetTile(new Vector3Int(i, j, 0), t);
                    worldMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);

                    worldMap.SetColor(new Vector3Int(i, j, 0), Color.Lerp(deconColor, conColor, mapImage.texture.GetPixel(i, j).r));

                    tiles[i][j] = new WorldTile(i, j, this, mapImage.texture.GetPixel(i,j).r);
                }

            }
        }
    }

    public WorldTile[][] GetWorldTiles()
    {
        return tiles;
    }

    public Tilemap GetWorldMap()
    {
        return worldMap;
    }

    public Color GetConColor()
    {
        return conColor;
    }

    public Color GetDeconColor()
    {
        return deconColor;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    public WorldTileController GetTileController()
    {
        return tileController;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class WorldTile
{
    WorldManager world;

    int x;
    int y;
    bool purified;

    float contStren; //strength of contamination for this tile. set by world
    float contLevel; //current level of contamination for the tile.
    float targetCont = 1f; //the target level for contamination on this tile
    bool transitioning; //if the tile is being contaminated/decontaminated

    bool hasObject;
    PlacableObject placedObject;

    List<Flower> activeFlowers;

    public WorldTile(int xPos, int yPos, WorldManager worldManager, float contaminationStrength)
    {
        x = xPos; 
        y = yPos;
        world = worldManager;
        contStren = contaminationStrength;
        contLevel = contStren;
        targetCont = contStren;

        activeFlowers = new List<Flower>();
    }

    public int GetX()
    {
        return x;
    }

    public int GetY() 
    { 
        return y; 
    }

    public bool GetPurified()
    {
        return purified;
    }

    public void PlaceObject(PlacableObject obj)
    {
        hasObject = true;
        placedObject = obj;
    }

    public void DestroyObject()
    {
        hasObject = false;
        placedObject = null;
    }

    public bool GetHasObject()
    {
        return hasObject;
    }

    public void AddFlower(Flower f)
    {
        //put flower into active flowers list
        activeFlowers.Add(f);

        SetTargetCont(0);
    }

    public void RemoveFlower(Flower f)
    {
        //reomve flwer from active flowers list
        activeFlowers.Remove(f);

        Debug.Log(activeFlowers.Count);

        //find a new max contLevel
        if (activeFlowers.Count == 0)
        {
            SetTargetCont(GetContStrength());
        }
    }

    Vector2Int GetRelativePos(Vector2Int flowerPos)
    {
        Vector2Int tilePos = new Vector2Int(x,y);

        return tilePos - flowerPos;
    }

    public List<Flower> GetActiveFlowers()
    {
        return activeFlowers;
    }
    public float GetContLevel()
    {
        return contLevel;
    }

    public void SetContLevel(float contLevel)
    {
        this.contLevel = contLevel;
    }

    public float GetContStrength()
    {
        return contStren;
    }

    public bool GetTransitioning()
    {
        return transitioning;
    }

    public void ChangeContLevel(float amt)
    {
        //1 = contaminated, 0 = decontaminated
        contLevel += amt;
    }

    public void BeginDecon()
    {
        transitioning = true;
        purified = true;

        //update place object's contamination
        placedObject?.UpdateContamination();
    }

    public void EndDecon()
    {
        transitioning = false;
    }

    public void BeginCon()
    {
        transitioning = true;
        purified = false;

        //update placed object's contamination
        placedObject?.UpdateContamination();
    }
    
    public void EndCon()
    {
        transitioning = false;
    }

    public void SetTargetCont(float tc)
    {
        transitioning = false;

        targetCont = tc;

        if(targetCont < contLevel)
        {
            //decontaminate
            world.GetTileController().StartCoroutine(world.GetTileController().Decontaminate(this));
        }
        else if(targetCont > contLevel)
        {

            //contaminate
            world.GetTileController().StartCoroutine(world.GetTileController().Contaminate(this));
        }
    }

    public float GetTargetCont()
    {
        return targetCont;
    }

    public PlacableObject GetPlacedObject()
    {
        return placedObject;
    }
}

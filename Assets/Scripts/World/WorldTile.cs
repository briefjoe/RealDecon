using System.Collections.Generic;
using UnityEngine;

public class WorldTile
{
    WorldManager world;

    int x;
    int y;
    bool purified;

    float contStren; //strength of contamination for this tile. set by world
    float contLevel; //level of contamination for this tile. set by flower
    float targetCont = 1f; //the target level for contamination on this tile
    bool contaminating; //if the tile is being contaminated
    bool decontaminating; //if the tile is being decontaminted

    bool hasObject;
    PlacableObject placedObject;

    List<Flower> activeFlowers;
    Flower mainFlower;

    void Start()
    {
        //activeFlowers = new List<Flower>();
    }

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
        placedObject.Destroy();

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

        /*//check for main flower
        if(mainFlower = null)
        {
            mainFlower = f;
        }
        else
        {
            //if(mainFlower.contstren < f.constren)
            //change the tile's target contamination level
        }*/
    }

    public void RemoveFlower(Flower f)
    {
        //reomve flwer from active flowers list
        activeFlowers.Remove(f);

        /*if (mainFlower.Equals(f))
        {
            if (activeFlowers.Count > 0)
            {
                mainFlower = activeFlowers[0];


                foreach (Flower tmp in activeFlowers)
                {

                }
            }
            else
            {
                mainFlower = null;
            }
        }*/
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

    public void ChangeContLevel(float amt)
    {
        //1 = contaminated, 0 = decontaminated
        contLevel += amt;
    }

    public bool GetContaminating()
    {
        return contaminating;
    }

    public bool GetDecontaminating()
    {
        return decontaminating;
    }

    public void BeginDecon()
    {
        decontaminating = true;
        purified = true;
    }

    public void EndDecon()
    {
        decontaminating = false;
    }

    public void BeginCon()
    {
        contaminating = true;
        purified = false;
    }
    
    public void EndCon()
    {
        contaminating = false;
    }
}

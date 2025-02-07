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
    bool converting; //if the tile is being contaminated/decontaminated

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
        converting = false;

        //activeFlowers.Add(f);


        /*//check for main flower
        if(mainFlower = null)
        {
            mainFlower = f;
        }
        else
        {
            //if(mainFlower.contstren < f.constren)
        }*/
    }

    public void RemoveFlower(Flower f)
    {
        //reomve flwer from active flowers list
        converting = false;

        //activeFlowers.Remove(f);


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

    public bool GetConverting()
    {
        return converting;
    }

    public void BeginDecon()
    {
        converting = true;
        purified = true;
    }

    public void EndDecon()
    {
        converting = false;
    }

    public void BeginCon()
    {
        converting = true;
        purified = false;
    }
    
    public void EndCon()
    {
        converting = false;
    }
}

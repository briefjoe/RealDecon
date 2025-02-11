using System;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("In Scene")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer playerSprite;
    

    [Header("Player Attributes")]
    [SerializeField] float speed = 10f;
    [SerializeField] float curContLevel;
    [SerializeField] float maxContLevel;
    [SerializeField] Flower flower;
    [SerializeField] Conaminable contam;

    float x = 0f;
    float y = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if(contam.GetMaxCon() <= contam.GetContamLevel())
        {
            //player dies!!!!!!!!!!!!!!
        }

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(x * speed, y * speed);
    }

    public Vector2Int GetPos()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    /*public void Interact(int posX, int posY)
    {
        //when the player clicks
        //use the object that the player has currently selected (right now, just flowers)

        /*if (!worldManager.GetWorldTiles()[posX][posY].GetHasObject())
        {

            PlacableObject f = Instantiate(flower);

            f.Place(worldManager, posX, posY);
        }*

        if (worldManager.GetWorldTiles()[posX][posY].GetHasObject())
        {
            //check if object is interactable
            //call the interact function on the object
        }
    }

    //TMP DESTROY FUNCTION -> Things will only get destroyed if the player is holding a tool
    public void DestroyObject(int posX, int posY)
    {
        //check if flower
        if (worldManager.GetWorldTiles()[posX][posY].GetHasObject())
        {
            worldManager.GetWorldTiles()[posX][posY].DestroyObject();
        }
    }*/
}
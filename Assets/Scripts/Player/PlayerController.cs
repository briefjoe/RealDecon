using System;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("In Scene")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] WorldManager worldManager;
    

    [Header("Player Attributes")]
    [SerializeField] float speed = 10f;
    [SerializeField] float curContLevel;
    [SerializeField] float maxContLevel;
    [SerializeField] Flower flower;

    float x = 0f;
    float y = 0f;

    public float progValue = 0f; //percentage of contamination level
    public Color contamColor = Color.white; //color filter to apply to player sprite

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

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(x * speed, y * speed);
    }

    public void ContaminatePlayer(float amount)
    {
        if (curContLevel < maxContLevel)
        {
            curContLevel = Mathf.Clamp(curContLevel + amount, 0, maxContLevel);

            ContaminationColor();
        }

        if(curContLevel >= maxContLevel)
        {
            //THE PLAYER DIES
        }
    }

    void ContaminationColor()
    {
        contamColor = Color.Lerp(Color.white, worldManager.GetConColor(), curContLevel / maxContLevel);
        playerSprite.color = contamColor;
    }

    public Vector2Int GetPos()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public void Interact(int posX, int posY)
    {
        //when the player clicks
        //use the object that the player has currently selected (right now, just flowers)

        if (!worldManager.GetWorldTiles()[posX][posY].GetHasObject())
        {

            PlacableObject f = Instantiate(flower);

            f.Place(worldManager, posX, posY);
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
    }
}
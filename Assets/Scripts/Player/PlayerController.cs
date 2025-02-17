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
        if (!Global.inMenu && !Global.isPaused)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }

        if(contam.GetMaxCon() <= contam.GetContamLevel())
        {
            //player dies!!!!!!!!!!!!!!
        }

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(x * speed, y * speed);
    }

    public SpriteRenderer GetPlayerSprite()
    {
        return playerSprite;
    }
}
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player current;
    public Rigidbody2D body;
    public float speed;


    public bool hasKey;

    //singleton
    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //player movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        body.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);

        //change sprite rotation depending on which way going
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); //facing right
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); //facing left
        }
        

        if(moveInput.y > 0)
        {
            //change sprite to face forward
        }
        else if(moveInput.y < 0)
        {
            //change sprite to face us
        }
    }
}

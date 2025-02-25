using UnityEngine;

namespace game 
{
public class Player : MonoBehaviour
{
    public static Player current;
    public Rigidbody2D body;
    public float speed;
    public float radius;

    public GameObject spotlight;

    public Sprite facingRight; 
    public Sprite facingLeft;  
    public Sprite facingUp;    
    public Sprite facingDown;  

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
        MovePlayer();


        ChangeSpriteDirection();
        //MousePositionCaclulate();

        UseItem();


        InteractNearby();

        ControlLight();

    }

    void ControlLight()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPos.z = 0;
        Vector3 spotlightPosition = spotlight.transform.position;
        Vector3 direction = (mouseWorldPos - spotlightPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spotlight.transform.rotation = Quaternion.Euler(-angle, 90, 0);
    }

    void ChangeSpriteDirection()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //change sprite rotation depending on which way going
        if (moveInput.x > 0)
        {
            GetComponent<SpriteRenderer>().sprite = facingRight;
        }
        else if (moveInput.x < 0)
        {
            GetComponent<SpriteRenderer>().sprite = facingLeft;
        }


        if (moveInput.y > 0)
        {
            GetComponent<SpriteRenderer>().sprite = facingUp;
        }
        else if (moveInput.y < 0)
        {
            GetComponent<SpriteRenderer>().sprite = facingDown;
        }

        ChangeSpriteDirection2();
    }

    
    void ChangeSpriteDirection2()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector2 direction = mouseWorldPos - transform.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
        {
            if (direction.x > 0) 
            {
                GetComponent<SpriteRenderer>().sprite = facingRight;
            }
            else 
            {
                GetComponent<SpriteRenderer>().sprite = facingLeft;
            }
        }
        else 
        {
            if (direction.y > 0) 
            {
                GetComponent<SpriteRenderer>().sprite = facingUp;
            }
            else 
            {
                GetComponent<SpriteRenderer>().sprite = facingDown;
            }
        }
    }
    

    void MousePositionCaclulate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; 
        Vector3 playerPosition = transform.position;
        playerPosition.z = 0;  
        if (mouseWorldPos.x > playerPosition.x)
        {
            Debug.Log("Mouse is to the right of the player (X-axis).");
        }
        else if (mouseWorldPos.x < playerPosition.x)
        {
            Debug.Log("Mouse is to the left of the player (X-axis).");
        }
        else
        {
            Debug.Log("Mouse is aligned with the player on the X-axis.");
        }
        if (mouseWorldPos.y > playerPosition.y)
        {
            Debug.Log("Mouse is above the player (Y-axis).");
        }
        else if (mouseWorldPos.y < playerPosition.y)
        {
            Debug.Log("Mouse is below the player (Y-axis).");
        }
        else
        {
            Debug.Log("Mouse is aligned with the player on the Y-axis.");
        }
    }

    void MovePlayer()
    {
        //player movement
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        body.velocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
    }

    /*
    void MovePlayer()
    {
        // Get player input from WASD
        float moveX = Input.GetAxisRaw("Horizontal"); // A (-1) / D (1)
        float moveY = Input.GetAxisRaw("Vertical");   // S (-1) / W (1)

        // Get mouse position
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Determine the facing direction (same as ChangeSpriteDirection)
        Vector2 direction = mouseWorldPos - transform.position;

        Vector2 forward = Vector2.zero;
        Vector2 right = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) // Horizontal priority
        {
            if (direction.x > 0) // Facing Right
            {
                forward = Vector2.right;
                right = Vector2.down;
            }
            else // Facing Left
            {
                forward = Vector2.left;
                right = Vector2.up;
            }
        }
        else // Vertical priority
        {
            if (direction.y > 0) // Facing Up
            {
                forward = Vector2.up;
                right = Vector2.right;
            }
            else // Facing Down
            {
                forward = Vector2.down;
                right = Vector2.left;
            }
        }

        // Move player relative to facing direction
        Vector2 moveDirection = (forward * moveY + right * moveX).normalized;
        body.velocity = moveDirection * speed;
    }
    */

    void UseItem()
    {
        //if F, use current selected item
        //if F, turn on light and switch between settings
        if (Input.GetKeyUp(KeyCode.F))
        {

        }
    }

    void InteractNearby()
    {

        //if Space, interact with an object
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //check what is nearby
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            if (colliders.Length > 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    //check that collider is not players own collider
                    if (collider.gameObject != gameObject)
                    {
                        //if has interactable component, then call interact method
                        Debug.Log("Overlap with " + collider.name);
                        if (collider.gameObject.TryGetComponent(out Interactable otherIsInteractable))
                        {
                            otherIsInteractable.Interact();
                        }
                    }
                }
            }
        }

    }

}
}

using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player current;
    public Rigidbody2D body;
    public float speed;
    public float radius;


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
            transform.localScale = new Vector3(2, 2, 2); //facing right
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2); //facing left
        }
        

        if(moveInput.y > 0)
        {
            //change sprite to face forward
        }
        else if(moveInput.y < 0)
        {
            //change sprite to face us
        }


        //if F, turn on light and switch between settings
        if(Input.GetKeyUp(KeyCode.F))
        {

        }


        //if Space, interact with an object
        if(Input.GetKeyUp(KeyCode.Space))
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

    void OnDrawGizmos()
    {
        // Visualize the sphere in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}

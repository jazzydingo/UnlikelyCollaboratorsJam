using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player current;
    public Rigidbody2D body;
    public float speed;

    private void Awake()
    {
        if(current == null)
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
        body.linearVelocity = new Vector3(InputSystem.actions["Move"].ReadValue<Vector2>().x * speed, InputSystem.actions["Move"].ReadValue<Vector2>().y * speed, 0);

        //change sprite rotation depending on which way going
    }
}

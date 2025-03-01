using UnityEngine;
using AK.Wwise;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        public Animator playerAnim;
        public bool noAnim;
        public bool footstepSound;

        public GameObject footstepObj;
        private GameObject soundObj;

        public RectTransform bar;

        private float elapsedTime;


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
            noAnim = true;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            MovePlayer();

            StopAnim();
             
            if(moveInput.x == 0 && moveInput.y == 0 && noAnim)
            {
                noAnim = true;
                //not moving, control direction accordingto light
                ChangeSpriteDirection2();
                playerAnim.enabled = false;
            }
            else if (moveInput.x != 0 || moveInput.y != 0 && noAnim)
            {
                playerAnim.enabled = true;
                noAnim = false;
                //else moving
                ChangeSpriteDirection();
            }

            SwitchLight();


            //MousePositionCaclulate();



            InteractNearby();

            
            ControlLight();

            PlayFootsteps();


            if(SceneManager.GetActiveScene().buildIndex > 6)
            {
                KeepLightLow();
            }
            

        }

        void KeepLightLow()
        {
            if (this.GetComponentInChildren<RevealLightOrb>().isOn)
            {
                bar.gameObject.SetActive(true);
                bar.gameObject.GetComponent<Image>().color = Color.red;
                LightCountDown();
            }
            else
            {
                //reset image width
                //set to green
                elapsedTime = 0;

                bar.gameObject.GetComponent<Image>().color = Color.green;

                bar.sizeDelta = new Vector2(500, bar.sizeDelta.y);
            }
        }

        void LightCountDown()
        {
            
            elapsedTime += Time.deltaTime;
            float newWidth = Mathf.Lerp(500, 1, elapsedTime / 8f); //divide by total time

            bar.sizeDelta = new Vector2(newWidth, bar.sizeDelta.y);

            if (elapsedTime >= 8f)
            {
                elapsedTime = 0;
                bar.gameObject.SetActive(false); 
            }

            if(bar.sizeDelta.x == 1)
            {
                Debug.Log("lose");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        

        void PlayFootsteps()
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && !footstepSound)
            {
                //moving, play footsteps
                footstepSound = true;
                if(soundObj == null)
                {

                    soundObj = Instantiate(footstepObj);
                }


            }
            if(footstepSound && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                //stop sound
                Destroy(soundObj);
                footstepSound = false;
            }
        }

        void SwitchLight()
        {
            if(Input.GetKeyUp(KeyCode.F))
            {
                //switch light mode
                this.GetComponentInChildren<RevealLightOrb>()._shouldSwitchState = !this.GetComponentInChildren<RevealLightOrb>()._shouldSwitchState;
            }
        }

        void StopAnim()
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                playerAnim.SetBool("Up", false);
                GetComponent<SpriteRenderer>().sprite = facingUp;
                noAnim = true;
            }
            else if(Input.GetKeyUp(KeyCode.A))
            {
                playerAnim.SetBool("Left", false);
                GetComponent<SpriteRenderer>().sprite = facingLeft;
                noAnim = true;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                playerAnim.SetBool("Down", false);
                GetComponent<SpriteRenderer>().sprite = facingDown;
                noAnim = true;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                playerAnim.SetBool("Right", false);
                GetComponent<SpriteRenderer>().sprite = facingRight;
                noAnim = true;
            }
        }

        void ControlLight()
        {
            if(spotlight.activeSelf && this.GetComponentInChildren<RevealLightOrb>().isOn)
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
                mouseWorldPos.z = 0;
                Vector3 spotlightPosition = spotlight.transform.position;
                Vector3 direction = (mouseWorldPos - spotlightPosition).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spotlight.transform.rotation = Quaternion.Euler(-angle, 90, 0);
            }
            
        }

        void ChangeSpriteDirection()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //change sprite rotation depending on which way going
            if (moveInput.x > 0)
            {
                GetComponent<SpriteRenderer>().sprite = facingRight;
                //play anim
                playerAnim.SetBool("Right", true);
                playerAnim.SetBool("Left", false);
                playerAnim.SetBool("Up", false);
                playerAnim.SetBool("Down", false);
                Debug.Log("right");
            }
            else if (moveInput.x < 0)
            {
                GetComponent<SpriteRenderer>().sprite = facingLeft;
                //play anim
                playerAnim.SetBool("Left", true);
                playerAnim.SetBool("Right", false);
                playerAnim.SetBool("Up", false);
                playerAnim.SetBool("Down", false);

                Debug.Log("left");
            }


            if (moveInput.y > 0)
            {
                GetComponent<SpriteRenderer>().sprite = facingUp;
                //play anim
                playerAnim.SetBool("Right", false);
                playerAnim.SetBool("Left", false);
                playerAnim.SetBool("Up", true);
                playerAnim.SetBool("Down", false);
            }
            else if (moveInput.y < 0)
            {
                GetComponent<SpriteRenderer>().sprite = facingDown;
                //play anim
                playerAnim.SetBool("Right", false);
                playerAnim.SetBool("Left", false);
                playerAnim.SetBool("Up", false);
                playerAnim.SetBool("Down", true);
            }
            


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

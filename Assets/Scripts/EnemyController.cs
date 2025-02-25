using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class EnemyController : MonoBehaviour
    {
        public Rigidbody2D body;
        public bool chase;
        public int speed;

        // Start is called before the first frame update
        void Start()
        {
            chase = true;
        }

        // Update is called once per frame
        void Update()
        {
            ChasePlayer();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.TryGetComponent(out RevealLightOrb otherIsLight))
            {
                //light is on this obj
                //stop moving 
                chase = false;
            }

            //if its a player, game over reset corridor scene
            if(other.gameObject == Player.current.gameObject)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out RevealLightOrb otherIsLight))
            {
                //light is not on this obj
                //chase player
                chase = true;
            }
        }

        void ChasePlayer()
        {
            if(chase)
            {
                Vector2 direction = (Player.current.transform.position - transform.position).normalized;
                body.velocity = direction * speed;
            }
            else
            {
                body.velocity = new Vector2(0, 0);
            }
            
        }
    }
}

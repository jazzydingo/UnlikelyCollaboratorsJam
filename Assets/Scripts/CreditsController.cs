using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class CreditsController : MonoBehaviour
    {
        private Rigidbody2D rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            //this.transform.position = new Vector2(this.transform.position.x - 2.5f * Time.deltaTime, this.transform.position.y);

        }

        void FixedUpdate()
        {
            rb.velocity = new Vector2(-2.5f, 0);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //
            Debug.Log("end");
            SceneManager.LoadScene(0);
        }
    }
}

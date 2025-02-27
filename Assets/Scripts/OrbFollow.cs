using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class OrbFollow : MonoBehaviour
    {
        public Transform player; 
        public float maxDistance = 3f;
        public static OrbFollow instance;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

            
            Vector3 direction = (mouseWorldPos - player.position).normalized;

            
            Vector3 targetPosition = player.position + direction * Mathf.Min(Vector3.Distance(mouseWorldPos, player.position), maxDistance);

            
            transform.position = targetPosition;

            
        }
    }
}

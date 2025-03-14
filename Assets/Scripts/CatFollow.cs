using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class CatFollow : MonoBehaviour
    {
        public float minDistance;
        

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            float distance = Vector3.Distance(this.transform.position, Player.current.transform.position);


            if (distance > minDistance)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, Player.current.transform.position, 1.5f * Time.deltaTime);
            }
        }
    }
}

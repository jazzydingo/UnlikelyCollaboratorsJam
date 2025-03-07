using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class MusicController : MonoBehaviour
    {
        public int indexToDestroy;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        void Awake()
        {
            

            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if(SceneManager.GetActiveScene().buildIndex == indexToDestroy)
            {
                Destroy(this.gameObject);
            }




        }
    }
}


//0 - menu music
//1 - dialogue, none
//2-3-4, ambience
//5 dialogue, none
//6-7 ambience
//8 - scary bedroom
//9-10 scary hallway
//11-12-13 - scary room
//14 - scary hallway
//15 dialogue none
//16 ending menu music
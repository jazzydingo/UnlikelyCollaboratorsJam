using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class SceneController : MonoBehaviour
    {
        public int index;
        public bool goNext;

        public static SceneController instance;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {

            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
            if(goNext)
            {
                
                    goNext = false;
                    NextScene();
                
            }
        }

        public void NextScene()
        {
            SceneManager.LoadScene(index);
        }

    }

}

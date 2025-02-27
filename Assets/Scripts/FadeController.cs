using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace game
{
    public class FadeController : MonoBehaviour
    {

        public static FadeController current;
       
        public float elapsedTime;
        public bool fadingToBlack;
        public bool fadingIntoScene;
    
        public float fadeDuration;
        public Image black;

        public bool startDialogue;
        public GameObject dialogueObj;


        void Awake()
        {
            if (current == null)
            {
                current = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        

        // Update is called once per frame
        void Update()
        {
            

            if (fadingToBlack) //end of scene, fade to black was fadingIn
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

                Color tempColor = black.color;
                tempColor.a = alpha;
                black.color = tempColor;

                if (alpha >= 1f)
                {
                    fadingToBlack = false; 
                    elapsedTime = 0f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else if(fadingIntoScene) //black fades out, start of scene
            {
                
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                Color tempColor = black.color;
                tempColor.a = alpha;
                black.color = tempColor;

                if (alpha <= 0f)
                {
                    fadingIntoScene = false;
                    elapsedTime = 0f;
                    if(startDialogue)
                    {
                        dialogueObj.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
    public class FadeController : MonoBehaviour
    {

        public static FadeController current;
       
        public float elapsedTime;
        public bool fadingIn;
        public bool fadingOut;
    
        public float fadeDuration;
        public Image black;

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
            

            if (fadingIn) //end of scene, fade to black
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

                Color tempColor = black.color;
                tempColor.a = alpha;
                black.color = tempColor;

                if (alpha >= 1f)
                {
                    fadingIn = false; 
                    elapsedTime = 0f;
                }
            }
            else if(fadingOut) //black fades out, start of scene
            {
                
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                Color tempColor = black.color;
                tempColor.a = alpha;
                black.color = tempColor;

                if (alpha <= 0f)
                {
                    fadingOut = false;
                    elapsedTime = 0f;
                }
            }
        }
    }
    
}

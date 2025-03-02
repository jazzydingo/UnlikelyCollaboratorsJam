using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class ExclamationController : MonoBehaviour
    {
        public bool exclamation;
        private float elapsedTime;
        public float fadeTime;
        public bool inactive;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!exclamation && !inactive)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);

                Color tempColor = this.GetComponent<SpriteRenderer>().color;
                tempColor.a = alpha;
                this.GetComponent<SpriteRenderer>().color = tempColor;

                if (alpha <= 0f)
                {
                    inactive = true;
                    elapsedTime = 0f;
                }
            }
            else if(exclamation)
            {
                inactive = false;
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);

                Color tempColor = this.GetComponent<SpriteRenderer>().color;
                tempColor.a = alpha;
                this.GetComponent<SpriteRenderer>().color = tempColor;

                if (alpha >= 1f)
                {
                    exclamation = false;
                    elapsedTime = 0f;
                }
            }
            
            
        }
    }
}

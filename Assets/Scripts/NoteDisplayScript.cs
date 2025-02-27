using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace game
{
    public class NoteDisplayScript : MonoBehaviour
    {
        public TextMeshProUGUI note;
        public bool fadingIn;
        public float fadeDuration;
        public float elapsedTime;
        public GameObject paper;
        public GameObject dialogueObj;
        public bool hasFadedIn;
        
        void Update()
        {
            var dragScripts = FindObjectsOfType<DraggableNote>();
            bool isAnyDragEnabled = false;

            foreach (var script in dragScripts)
            {
                if (script is DraggableNote && script.enabled)
                {
                    isAnyDragEnabled = true;
                    break;
                }
            }


            if (!isAnyDragEnabled && !hasFadedIn)
            {

                fadingIn = true;
                
                    
                
            }

            if (fadingIn)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

                FadePaper(alpha);
                FadeNote(alpha);


                if (alpha >= 1f)
                {
                    fadingIn = false;
                    hasFadedIn = true;
                    //startDialogue
                    dialogueObj.GetComponentInChildren<DialogueController>().startDialogue = true;
                }
            }
        }

        void FadeNote(float alpha)
        {
            Color tempColor = note.color;
            tempColor.a = alpha;
            note.color = tempColor;
        }

        void FadePaper(float alpha)
        {
            Color tempColor = paper.GetComponent<SpriteRenderer>().color;
            tempColor.a = alpha;
            paper.GetComponent<SpriteRenderer>().color = tempColor;


        }
        
    }


    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game 
{
    public class Interactable : MonoBehaviour
    {
        public bool pickup;
        public GameObject prefab;

        public bool flashlight;
        public bool key;
        public bool bed;

        public Material defaultMaterial;
        public Material outlineMaterial;

        public int flashlightMode;

        public GameObject lightObj;

        public GameObject dialogueBox;



        // Start is called before the first frame update
        void Start()
        {
            flashlightMode = 0;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Interact()
        {
            //interact depending on what object it is
            if(pickup)
            {
                //add this game object to inventory (save this object as reference, set sprite of this object to next inventory slot)
                InventoryController.current.AddObject(this.gameObject);


                //allow object to be "used"
            }
            else
            {
                
                UseObject();
            }
        }

        public void UseObject()
        {
            if(flashlight)
            {
                Debug.Log("use flashlight");
                //code to use flashlight
                if(flashlightMode == 0)
                {
                    //turn flashlight on
                    Player.current.spotlight.gameObject.SetActive(true);
                    Player.current.spotlight.GetComponent<Light>().spotAngle = 70f;
                    Player.current.spotlight.GetComponent<Light>().color = Color.red;
                    flashlightMode++;
                }
                else if(flashlightMode == 1)
                {
                    //change flashlight mode
                    Player.current.spotlight.GetComponent<Light>().spotAngle = 24f;
                    Player.current.spotlight.GetComponent<Light>().color = Color.yellow;
                    flashlightMode++;
                }
                else if(flashlightMode == 2) 
                {
                    //turn flashlight off
                    Player.current.spotlight.gameObject.SetActive(false);
                    flashlightMode = 0;
                }
            }
            else if(key)
            {
                //code to use key to unlock door
            }
            else if(bed)
            {
                if(!dialogueBox.gameObject.GetComponentInParent<DialogueController>().choiceYes)
                {
                    dialogueBox.gameObject.SetActive(true);
                    dialogueBox.GetComponentInParent<DialogueController>().startDialogue = true;
                }
                
                
            }

            //other objects that need to be collected
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.GetComponent<Player>() != null)
            {
                this.gameObject.GetComponent<SpriteRenderer>().material = outlineMaterial;
            }



        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Player>() != null)
            {
                this.gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            }

        }
    }
}

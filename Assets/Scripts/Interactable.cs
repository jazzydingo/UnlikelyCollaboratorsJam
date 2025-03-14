using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace game 
{
    public class Interactable : MonoBehaviour
    {
        public bool pickup;
        public GameObject prefab;

        public bool flashlight;
        public bool key;
        public bool bed;
        public bool safe;

        public Material defaultMaterial;
        public Material outlineMaterial;

        public int flashlightMode;

        public GameObject lightObj;

        public GameObject dialogueBox;

        public bool note;

        public bool dialogue;

        public string line;

        public GameObject lineDialogue;
        public GameObject lineObj;
        public bool endOfDialogue;
        public bool isPlaying;
        public bool sound;
        public bool skip;

        public GameObject talkSound;
        public GameObject npcTalk;

        public GameObject safeUI;

        public GameObject unlockSFX;



        // Start is called before the first frame update
        void Start()
        {
            flashlightMode = 0;
            isPlaying = false;
            sound = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(lineObj != null && lineObj.activeSelf)
            {
                if (isPlaying && !sound)
                {
                    sound = true;
                    
                    talkSound = Instantiate(npcTalk);
                    
                }

                else if (Input.GetMouseButtonDown(0) && !endOfDialogue && isPlaying)
                {
                    skip = true;
                    Destroy(talkSound);
                    sound = false;

                }
                else if (Input.GetMouseButtonDown(0) && endOfDialogue && !isPlaying)
                {

                    lineObj.gameObject.SetActive(false);
                    if (Player.current != null)
                    {
                        Player.current.enabled = true;
                    }
                    if (OrbFollow.instance != null)
                    {
                        OrbFollow.instance.enabled = true;
                    }


                }
            }
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
            else if (dialogue)
            {
                if(!key || key && !safeUI.GetComponentInParent<SafeController>().solved)
                {
                    //pull up dialogue and display line
                    lineObj = Instantiate(lineDialogue);

                    isPlaying = false;

                    lineObj.gameObject.SetActive(true);

                    if (Player.current != null)
                    {
                        Player.current.enabled = false;
                    }
                    if (OrbFollow.instance != null)
                    {
                        OrbFollow.instance.enabled = false;
                    }

                    StartCoroutine(NextLine());
                }
                else if(key && safeUI.GetComponentInParent<SafeController>().solved)
                {
                    //pull up dialogue and display line
                    lineObj = Instantiate(lineDialogue);
                    line = "You used the key to open the padlock.";
                    

                    isPlaying = false;

                    lineObj.gameObject.SetActive(true);

                    if (Player.current != null)
                    {
                        Player.current.enabled = false;
                    }
                    if (OrbFollow.instance != null)
                    {
                        OrbFollow.instance.enabled = false;
                    }

                    StartCoroutine(NextLine());

                    //play unlock sound 
                    unlockSFX.GetComponent<AudioSource>().Play();
                    //add object to inventory

                }
            }
            
            else if(bed)
            {
                if(!dialogueBox.gameObject.GetComponentInParent<DialogueController>().choiceYes)
                {
                    dialogueBox.gameObject.SetActive(true);
                    dialogueBox.GetComponentInParent<DialogueController>().startDialogue = true;
                }
                
                
            }
            else if(safe)
            {
                safeUI.gameObject.SetActive(true);
            }

            //other objects that need to be collected
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.GetComponent<Player>() != null)
            {
                //this.gameObject.GetComponent<SpriteRenderer>().material = outlineMaterial;
                //exclamation mark above players head
                Player.current.GetComponentInChildren<ExclamationController>().exclamation = true;
            }



        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Player>() != null)
            {
                //this.gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            }

        }

        IEnumerator NextLine()
        {
            
                lineObj.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
                skip = false;
                isPlaying = true;

                foreach (char letter in line)
                {

                    lineObj.GetComponentInChildren<TextMeshProUGUI>().text += letter;

                    if (!skip)
                    {
                        yield return new WaitForSeconds(0.02f);
                    }
                    else
                    {
                        break;
                    }
                }
                isPlaying = false;
                Destroy(talkSound);
                sound = false;
                lineObj.GetComponentInChildren<TextMeshProUGUI>().text = line;
                
                    skip = false;
                    endOfDialogue = true;
                    // reset index?
                    if (Player.current != null)
                    {
                        Player.current.enabled = true;
                    }
                    if (OrbFollow.instance != null)
                    {
                        OrbFollow.instance.enabled = true;
                    }
                    yield return null;
                
            

        }
    }
}

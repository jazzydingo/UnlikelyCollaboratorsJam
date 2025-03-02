using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace game 
{
    public class DialogueController : MonoBehaviour
    {
        public TextMeshProUGUI dialogue;
        public bool skip;
        public string[] dialogueLines;
        public int index;
        public float textSpeed;
        public bool endOfDialogue;
        public GameObject dialogueObj;
        public bool isPlaying;

        public GameObject npcTalk;
        public GameObject talkSound;

        public bool choice;
        public GameObject choiceBox;

        public bool choiceYes;
        public bool startDialogue; //starts dialogue on scene start

        public bool endAfterDialogue;

        public bool sound;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            skip = false;
            dialogue.text = string.Empty;
            dialogueObj.gameObject.SetActive(false);
            if(Player.current != null)
            {
                Player.current.enabled = true;
            }
            if(OrbFollow.instance != null)
            {
                OrbFollow.instance.enabled = true;
            }
            if(choiceBox != null)
            {
                choiceBox.gameObject.SetActive(false);
            }
            
            choiceYes = false;
            isPlaying = true;
        }

        public void Yes()
        {
            Debug.Log("yes");
            choiceYes = true;
            if (Player.current != null)
            {
                Player.current.enabled = true;
            }
            if (OrbFollow.instance != null)
            {
                OrbFollow.instance.enabled = true;
            }
            dialogueObj.gameObject.SetActive(false);
            FadeController.current.fadingToBlack = true;
            FadeController.current.fadingIntoScene = false;
        }

        public void No()
        {
            Debug.Log("no");
            choiceYes = false;
            if (Player.current != null)
            {
                Player.current.enabled = true;
            }
            if (OrbFollow.instance != null)
            {
                OrbFollow.instance.enabled = true;
            }
            dialogueObj.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(startDialogue)
            {
                startDialogue = false;
                isPlaying = false;

                dialogueObj.gameObject.SetActive(true);

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

            if(isPlaying && !sound)
            {
                sound = true;
                if(talkSound == null)
                {
                    talkSound = Instantiate(npcTalk);
                }
                
            }

            if (Input.GetMouseButtonDown(0) && !endOfDialogue && !isPlaying)
            {
                dialogueObj.gameObject.SetActive(true);
                skip = true;
                Destroy(talkSound);
                sound = false;
                
                StartCoroutine(NextLine());
            }
            else if(Input.GetMouseButtonDown(0) && !endOfDialogue && isPlaying)
            {
                skip = true;
                Destroy(talkSound);
                sound = false;

            }
            else if (Input.GetMouseButtonDown(0) && endOfDialogue && !isPlaying)
            {
                if(!choice)
                {
                    dialogueObj.gameObject.SetActive(false);
                    if (Player.current != null)
                    {
                        Player.current.enabled = true;
                    }
                    if (OrbFollow.instance != null)
                    {
                        OrbFollow.instance.enabled = true;
                    }
                    //next scene
                    if(endAfterDialogue)
                    {
                        if (GameObject.FindWithTag("scene") != null)
                        {
                            GameObject.FindWithTag("scene").GetComponent<SceneController>().goNext = true;
                        }
                        else
                        {
                            SceneController.instance.goNext = true;
                        }

                    }
                }
            }
            else
            {

            }
        }

        IEnumerator NextLine()
        {
            if (!endOfDialogue)
            {
                dialogue.text = string.Empty;
                skip = false;
                isPlaying = true;

                foreach (char letter in dialogueLines[index])
                {
                    dialogue.text += letter;

                    if (!skip)
                    {
                        yield return new WaitForSeconds(textSpeed);
                    }
                    else
                    {
                        break;
                    }
                }
                isPlaying = false;
                Destroy(talkSound);
                sound = false;
                dialogue.text = dialogueLines[index];
                if (index < dialogueLines.Length - 1)
                {
                    index++;
                    skip = false;
                    NextLine();
                }
                else
                {
                    if(choice)
                    {
                        choiceBox.gameObject.SetActive(true);
                    }    
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
    }
}

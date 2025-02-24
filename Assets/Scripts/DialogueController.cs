using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Game 
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skip = false;
        dialogue.text = string.Empty;
        dialogueObj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !endOfDialogue && !isPlaying)
        {

            dialogueObj.gameObject.SetActive(true);
            skip = true;
            StartCoroutine(NextLine());
        }
        else if(Input.GetMouseButtonDown(0) && !endOfDialogue && isPlaying)
        {
            skip = true;
        }
        else if (Input.GetMouseButtonDown(0) && endOfDialogue && !isPlaying)
        {
            dialogueObj.gameObject.SetActive(false);
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
            dialogue.text = dialogueLines[index];
            if (index < dialogueLines.Length - 1)
            {
                index++;
                skip = false;
                NextLine();
            }
            else
            {
                skip = false;
                endOfDialogue = true;
                yield return null;
            }
        }

    }
}
}

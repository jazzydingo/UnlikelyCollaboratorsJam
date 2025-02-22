using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool pickup;
    public GameObject prefab;
    public bool flashlight;
    public bool key;
    public Material defaultMaterial;
    public Material outlineMaterial;


    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void UseObject()
    {
        if(flashlight)
        {
            //code to use flashlight
        }
        else if(key)
        {
            //code to use key to unlock door
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

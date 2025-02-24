using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool pickup;
    public GameObject prefab;


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
}

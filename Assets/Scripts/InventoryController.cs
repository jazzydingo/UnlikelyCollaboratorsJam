using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;

    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject obj5;

    public GameObject activeObj;

    public GameObject highlight;

    public int index;

    public GameObject[] inventoryArray;

    public int nextEmpty;

    // Start is called before the first frame update
    void Start()
    {
        activeObj = inventoryArray[index];
        highlight.transform.position = activeObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //highlight the current selected object
        activeObj = inventoryArray[index];
        highlight.transform.position = activeObj.transform.position;

        //change current object when scrollwheel
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(index == 0)
            {
                index = 4;
            }
            else
            {
                index--;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (index == 4)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

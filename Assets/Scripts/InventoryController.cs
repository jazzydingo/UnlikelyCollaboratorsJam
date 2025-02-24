using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public static InventoryController current;

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

    public Sprite empty;

    public GameObject activeObj;

    public GameObject highlight;

    public int index;

    public GameObject[] inventoryArray;
    public GameObject[] objArray;

    public int nextEmpty;



    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        activeObj = inventoryArray[index];
        highlight.transform.position = new Vector2(125, 1560);
        empty = inventoryArray[2].GetComponent<Image>().sprite;
        //Debug.Log("Highlight initial position: " + highlight.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        HighlightCurrent();

        ScrollToObject();

        UpdateSprites();

        //Debug.Log("Highlight initial position: " + highlight.transform.position);

    }

    void UpdateSprites()
    {
        //keep slot sprites updated
        for (int i = 0; i < inventoryArray.Length - 1; i++)
        {
            if (objArray[i] != null)
            {
                inventoryArray[i].GetComponent<Image>().sprite = objArray[i].GetComponent<SpriteRenderer>().sprite;
                //this line is for test purposes, color will be part of sprite in actual sprites
                inventoryArray[i].GetComponent<Image>().color = objArray[i].GetComponent<SpriteRenderer>().color;
            }
            else
            {
                inventoryArray[i].GetComponent<Image>().sprite = empty;
            }
        }

    }

    void ScrollToObject()
    {
        //change current object when scrollwheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (index == 0)
            {
                index = 4;
            }
            else
            {
                index--;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
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

    void HighlightCurrent()
    {
        //highlight the current selected object
        activeObj = inventoryArray[index];
        //this is causing issues at scene start, where the highlight box moves somewhere else initially
        highlight.transform.position = Vector3.Lerp(highlight.transform.position, activeObj.transform.position, 10f * Time.deltaTime);
    }

    public void AddObject(GameObject obj)
    {

        GameObject newObj = obj.GetComponent<Interactable>().prefab;
        objArray[nextEmpty] = newObj;
        nextEmpty++;

        //destroy this game object
        Destroy(obj);


    }
}

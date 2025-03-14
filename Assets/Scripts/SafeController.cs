using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace game
{
    public class SafeController : MonoBehaviour
    {
        public GameObject UIPanel;
        public TextMeshProUGUI first;
        public TextMeshProUGUI second;
        public TextMeshProUGUI third;
        public TextMeshProUGUI fourth;
        public int num1;
        public int num2;
        public int num3;
        public int num4;

        public bool solved;

        // Start is called before the first frame update
        void Start()
        {
            num1 = 0;
            num2 = 0;
            num3 = 0;
            num4 = 0;
            
        }

        // Update is called once per frame
        void Update()
        {
            first.text = "" + num1;
            second.text = "" + num2;
            third.text = "" + num3;
            fourth.text = "" + num4;

            if(num1 == 2 && num2 == 1 && num3 == 3 && num4 == 4)
            {
                Debug.Log("solved");
                solved = true;


            }

            
        }

        public void Solve()
        {
            if(solved)
            {
                //play sound

                UIPanel.gameObject.SetActive(false);
            }
        }

        public void OpenUI()
        {
            UIPanel.gameObject.SetActive(true);
        }

        public void CloseUI()
        {
            UIPanel.gameObject.SetActive(false);
        }

        public void FirstUp()
        {
            num1++;
            if(num1 == 10)
            {
                num1 = 0;
            }
            
           
        }

        public void FirstDown()
        {
            num1--;
            if(num1 == -1)
            {
                num1 = 9;
            }
        }

        public void SecondUp()
        {
            num2++;
            if (num2 == 10)
            {
                num2 = 0;
            }
        }

        public void SecondDown()
        {
            num2--;
            if (num2 == -1)
            {
                num2 = 9;
            }
        }

        public void ThirdUp()
        {
            num3++;
            if (num3 == 10)
            {
                num3 = 0;
            }
        }

        public void ThirdDown()
        {
            num3--;
            if (num3 == -1)
            {
                num3 = 9;
            }
        }

        public void FourthUp()
        {
            num4++;
            if (num4 == 10)
            {
                num4 = 0;
            }
        }

        public void FourthDown()
        {
            num4--;
            if (num4 == -1)
            {
                num4 = 9;
            }
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{

    public class DarkenLighten : MonoBehaviour
    {

        public Color darkColor;
        private Color tempColor;
        // Start is called before the first frame update
        void Start()
        {
            darkColor = GetComponent<SpriteRenderer>().color;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.TryGetComponent(out RevealLightOrb otherIsLight))
            {
                tempColor = GetComponent<SpriteRenderer>().color;
                float increaseAmount = 89f / 255f;

                tempColor.r = Mathf.Clamp01(tempColor.r + increaseAmount);
                tempColor.g = Mathf.Clamp01(tempColor.g + increaseAmount);
                tempColor.b = Mathf.Clamp01(tempColor.b + increaseAmount);

                GetComponent<SpriteRenderer>().color = tempColor;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out RevealLightOrb otherIsLight))
            {
                GetComponent<SpriteRenderer>().color = darkColor;
            }
        }
    }
}

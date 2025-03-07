using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

namespace game
{
    public class PlayFootsteps : MonoBehaviour
    {

        public GameObject obj;
        // Start is called before the first frame update
        void Start()
        {
            AkSoundEngine.PostEvent("Play_Footsteps", obj);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

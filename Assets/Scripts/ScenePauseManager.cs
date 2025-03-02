using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class ScenePauseManager : MonoBehaviour
    {

        
        void OnTriggerEnter2D(Collider2D other)
        {
            SceneController.instance.goNext = true;
        }
    }
}
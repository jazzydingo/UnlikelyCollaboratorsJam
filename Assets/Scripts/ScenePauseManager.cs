using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class ScenePauseManager : MonoBehaviour
    {

        
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.TryGetComponent(out Player otherIsPlayer))
            {
                SceneController.instance.goNext = true;
            }
            
        }
    }
}
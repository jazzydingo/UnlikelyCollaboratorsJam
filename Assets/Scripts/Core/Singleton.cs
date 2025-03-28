using UnityEngine;

namespace game {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        public static T Instance { get { return GetInstance(); } }

        private static T s_instance;
        private static readonly object s_threadLock = new();
        private static bool s_isQuitting = false;

        private static T GetInstance() {
            if (s_isQuitting) return null;
            if (s_instance != null) return s_instance;

            s_instance = FindAnyObjectByType<T>();
            if (s_instance != null) return s_instance;

            lock (s_threadLock) { CreateNewInstance(); }
            return s_instance;
        }

        private static void CreateNewInstance() {
            GameObject singletonObject = new() {
                name = $"{typeof(T).Name} (Persists)"
            };

            s_instance = singletonObject.AddComponent<T>();

            DontDestroyOnLoad(s_instance);
            // Debug.Log($"Singleton Instance {s_instance.GetInstanceID()} Initialized");
        }
    
        private void OnDestroy() {
            s_isQuitting = true;
        }
    }
}
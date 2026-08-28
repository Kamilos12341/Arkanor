using UnityEngine;

namespace Arkanor.Player
{
    public class PlayerReference : MonoBehaviour
    {
        public static PlayerReference Instance { get; private set; }

        public Transform Transform => transform;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
using TMPro;
using UnityEngine;

namespace Arkanor.UI
{

    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private GameObject prompt;

        public void Show()
        {
            prompt.SetActive(true);
        }

        public void Hide()
        {
            prompt.SetActive(false);
        }
    }
}
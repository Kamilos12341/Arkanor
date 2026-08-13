using TMPro;
using UnityEngine;

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
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicker : MonoBehaviour
{
    [SerializeField] private Button targetButton;

    public void ClickButton()
    {
        // Vérifie si le bouton est assigné et interactable
        if (targetButton != null && targetButton.interactable)
        {
            targetButton.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("Button is either null or not interactable");
        }
    }
}
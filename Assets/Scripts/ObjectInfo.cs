using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Add this component to objects that should display info
public class ObjectInfo : MonoBehaviour
{
    [System.Serializable]
    public class ObjectDetails
    {
        public string objectName;
        [TextArea(3, 10)]
        public string description;
        public string category;
        public string creationDate;
    }

    public ObjectDetails details;

    private void OnMouseDown()
    {
        InfoPopupManager.Instance.ShowPopup(details);
    }
}
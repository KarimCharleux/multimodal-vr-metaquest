using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPopupManager : MonoBehaviour
{
    public static InfoPopupManager Instance { get; private set; }

    [Header("Popup References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private RectTransform popupRectTransform;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private Button closeButton;
    
    [Header("Layout Settings")]
    [SerializeField] private float defaultWidth = 400f;
    [SerializeField] private float defaultHeight = 600f;
    [SerializeField] private float minWidth = 200f;
    [SerializeField] private float minHeight = 300f;
    [SerializeField] private float maxWidth = 800f;
    [SerializeField] private float maxHeight = 1000f;
    
    [Header("Animation")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        closeButton.onClick.AddListener(HidePopup);
        
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    private Vector2 CalculateScaledSize()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        float widthScale = screenWidth / 1920f;
        float heightScale = screenHeight / 1080f;
        
        float scaledWidth = defaultWidth * widthScale;
        float scaledHeight = defaultHeight * heightScale;
        
        scaledWidth = Mathf.Clamp(scaledWidth, minWidth, maxWidth);
        scaledHeight = Mathf.Clamp(scaledHeight, minHeight, maxHeight);
        
        return new Vector2(scaledWidth, scaledHeight);
    }

    public void ShowPopup(ObjectInfo.ObjectDetails details)
    {
        titleText.text = details.objectName;
        descriptionText.text = details.description;
        categoryText.text = $"Category: {details.category}";
        dateText.text = $"Created: \n {details.creationDate}";

        if (popupRectTransform != null)
        {
            Vector2 newSize = CalculateScaledSize();
            popupRectTransform.sizeDelta = newSize;
        }

        popupPanel.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = elapsedTime / fadeInDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (popupPanel.activeSelf && popupRectTransform != null)
        {
            Vector2 newSize = CalculateScaledSize();
            popupRectTransform.sizeDelta = newSize;
        }
    }
}
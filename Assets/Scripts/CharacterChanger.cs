using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Transform scrollContent;
    [SerializeField] private Button applyButton;
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Texture2D[] textures;
    [SerializeField] private int imagesPerRow = 3;
    [SerializeField] private float spacingFraction = 0.1f; 
    [SerializeField] private Color selectedColor = new Color(0.8f, 0.8f, 1f, 1f);
    [SerializeField] private Color normalColor = Color.white;

    private SkinnedMeshRenderer characterRenderer;
    private Texture2D currentTexture;
    private Texture2D selectedTexture;
    private Image currentSelectedImage;
    private float lastWidth;

    private GameObject[] characterModels;

    void Start()
    {
        SetupScrollView();
        SetupCharacters();
        characterRenderer = GameObject.Find("SimplePeople").GetComponentInChildren<SkinnedMeshRenderer>();

        if (textures != null && textures.Length > 0)
        {
            CreateTextureImages();
            SetupApplyButton();
            SetInitialTexture();
        }
        else
        {
            Debug.LogError("No textures assigned to the CharacterChanger script!");
        }
    }

    void Update()
    {
        if (scrollView != null && !Mathf.Approximately(lastWidth, scrollView.GetComponent<RectTransform>().rect.width))
        {
            UpdateLayoutSizes();
        }
    }

    void SetupScrollView()
    {
        if (scrollView != null)
        {
            // Configure ScrollRect
            scrollView.horizontal = false;
            scrollView.vertical = true;
            scrollView.movementType = ScrollRect.MovementType.Elastic;
            scrollView.elasticity = 0.1f;
            scrollView.inertia = true;
            scrollView.decelerationRate = 0.135f;

            // Setup content
            GridLayoutGroup grid = scrollContent.gameObject.GetComponent<GridLayoutGroup>();
            if (!grid)
            {
                grid = scrollContent.gameObject.AddComponent<GridLayoutGroup>();
            }

            ContentSizeFitter sizeFitter = scrollContent.gameObject.GetComponent<ContentSizeFitter>();
            if (!sizeFitter)
            {
                sizeFitter = scrollContent.gameObject.AddComponent<ContentSizeFitter>();
            }
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            UpdateLayoutSizes();
        }
    }

    void UpdateLayoutSizes()
    {
        if (scrollView == null) return;

        RectTransform scrollRectTransform = scrollView.GetComponent<RectTransform>();
        float viewportWidth = scrollRectTransform.rect.width;
        lastWidth = viewportWidth;

        // Calculate cell size based on viewport width and images per row
        float spacing = viewportWidth * spacingFraction / (imagesPerRow + 1);
        float cellSize = (viewportWidth - (spacing * (imagesPerRow + 1))) / imagesPerRow;

        GridLayoutGroup grid = scrollContent.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.cellSize = new Vector2(cellSize, cellSize);
            grid.spacing = new Vector2(spacing, spacing);
            grid.padding = new RectOffset(
                Mathf.RoundToInt(spacing),
                Mathf.RoundToInt(spacing),
                Mathf.RoundToInt(spacing),
                Mathf.RoundToInt(spacing)
            );
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = imagesPerRow;
        }

        // Update existing image sizes
        foreach (Transform child in scrollContent)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
            }
        }
    }

    void CreateTextureImages()
    {
        foreach (Transform child in scrollContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < textures.Length; i++)
        {
            Texture2D texture = textures[i];
            GameObject imageObj = Instantiate(imagePrefab, scrollContent);
            
            // Setup the image component
            Image image = imageObj.GetComponent<Image>();
            if (image != null)
            {
                // Create a sprite from the texture
                Sprite sprite = Sprite.Create(
                    texture, 
                    new Rect(0, 0, texture.width, texture.height), 
                    new Vector2(0.5f, 0.5f)
                );
                image.sprite = sprite;
                image.preserveAspect = true;
            }

            // Add click detection
            Button button = imageObj.GetComponent<Button>();
            if (button == null)
            {
                button = imageObj.AddComponent<Button>();
            }

            int index = i;
            button.onClick.AddListener(() => SelectTexture(index, image));
        }

        // Initial layout update
        UpdateLayoutSizes();
    }

    void SelectTexture(int index, Image clickedImage)
    {
        if (currentSelectedImage != null)
        {
            currentSelectedImage.color = normalColor;
        }

        selectedTexture = textures[index];
        currentSelectedImage = clickedImage;
        currentSelectedImage.color = selectedColor;

        characterRenderer.material.mainTexture = selectedTexture;
    }

    void SetupApplyButton()
    {
        if (applyButton != null)
        {
            applyButton.onClick.RemoveAllListeners();
            applyButton.onClick.AddListener(ApplyTexture);
        }
    }

    void SetupCharacters()
    {
        // Find the SimplePeople parent object
        Transform simplePeopleParent = GameObject.Find("SimplePeople").transform;
        
        // Get all child characters
        characterModels = new GameObject[simplePeopleParent.childCount];
        for (int i = 0; i < simplePeopleParent.childCount; i++)
        {
            characterModels[i] = simplePeopleParent.GetChild(i).gameObject;
            characterModels[i].SetActive(i == 0);
        }
    }

    void SetInitialTexture()
    {
        currentTexture = textures[0];
        characterRenderer.material.mainTexture = currentTexture;
    }

    void ApplyTexture()
    {
        if (selectedTexture != null)
        {
            currentTexture = selectedTexture;
            Debug.Log($"Applied texture: {currentTexture.name}");
        }
    }
}
using UnityEngine;

public class MobileVRController : MonoBehaviour
{
    [Header("VR Settings")]
    [SerializeField] private bool enableGyro = true;
    [SerializeField] private float gyroSmoothing = 0.1f;

    [Header("Object Manipulation")]
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private float maxSelectionDistance = 10f;
    [SerializeField] private Material selectedObjectMaterial;
    [SerializeField] private Material hoveredObjectMaterial;
    [SerializeField] private float objectHoldDistance = 2f;
    
    // Private variables
    private Camera mainCamera;
    private GameObject selectedObject;
    private GameObject hoveredObject;
    private Material originalMaterial;
    private Material originalHoverMaterial;
    private bool isHoldingObject = false;
    private Quaternion gyroInitialRotation;
    private bool gyroInitialized = false;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found!");
            enabled = false;
            return;
        }

        // Initialize gyroscope
        InitializeGyro();

        // Create hover material if not assigned
        if (hoveredObjectMaterial == null)
        {
            hoveredObjectMaterial = new Material(Shader.Find("Standard"));
            hoveredObjectMaterial.color = new Color(1f, 1f, 0f, 0.5f);
        }

        // Lock screen orientation to landscape
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Keep screen active
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void InitializeGyro()
    {
        if (enableGyro && SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            gyroInitialRotation = Quaternion.Euler(90f, 90f, 0f);
            gyroInitialized = true;
            Debug.Log("Gyroscope initialized");
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device");
        }
    }

    private void Update()
    {
        // Update camera rotation based on gyroscope
        if (gyroInitialized)
        {
            UpdateCameraRotation();
        }

        // Check for object under crosshair
        if (!isHoldingObject)
        {
            CheckHover();
        }

        // Handle touch input
        HandleTouch();

        // Update held object position
        if (isHoldingObject && selectedObject != null)
        {
            UpdateHeldObjectPosition();
        }
    }

    private void UpdateCameraRotation()
    {
        Quaternion gyroRotation = Input.gyro.attitude;
        Quaternion rotFix = new Quaternion(gyroRotation.x, gyroRotation.y, -gyroRotation.z, -gyroRotation.w);
        Quaternion targetRotation = gyroInitialRotation * rotFix;

        // Smoothly interpolate to target rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            gyroSmoothing
        );
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!isHoldingObject)
            {
                // Try to select object
                Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayer))
                {
                    SelectObject(hit.collider.gameObject);
                    isHoldingObject = true;
                }
            }
            else
            {
                // Place object
                DeselectObject();
                isHoldingObject = false;
            }
        }
    }

    private void UpdateHeldObjectPosition()
    {
        Vector3 targetPosition = mainCamera.transform.position + 
                               mainCamera.transform.forward * objectHoldDistance;

        selectedObject.transform.position = Vector3.Lerp(
            selectedObject.transform.position,
            targetPosition,
            Time.deltaTime * 10f
        );
    }

    private void CheckHover()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            if (hitObject != hoveredObject)
            {
                UnhoverCurrentObject();
                hoveredObject = hitObject;
                var renderer = hoveredObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    originalHoverMaterial = renderer.material;
                    renderer.material = hoveredObjectMaterial;
                }
            }
        }
        else
        {
            UnhoverCurrentObject();
        }
    }

    private void SelectObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            DeselectObject();
        }

        if (obj == hoveredObject)
        {
            UnhoverCurrentObject();
        }

        selectedObject = obj;
        var renderer = selectedObject.GetComponent<Renderer>();
        if (renderer != null && selectedObjectMaterial != null)
        {
            originalMaterial = renderer.material;
            renderer.material = selectedObjectMaterial;
        }
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            var renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }
            selectedObject = null;
        }
    }

    private void UnhoverCurrentObject()
    {
        if (hoveredObject != null)
        {
            var renderer = hoveredObject.GetComponent<Renderer>();
            if (renderer != null && originalHoverMaterial != null)
            {
                renderer.material = originalHoverMaterial;
            }
            hoveredObject = null;
        }
    }

    private void OnGUI()
    {
        // Draw crosshair
        float size = 20f;
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;
        
        GUI.color = hoveredObject != null ? Color.yellow : Color.white;
        GUI.DrawTexture(
            new Rect(centerX - size/2, centerY - 1, size, 2), 
            Texture2D.whiteTexture
        );
        GUI.DrawTexture(
            new Rect(centerX - 1, centerY - size/2, 2, size), 
            Texture2D.whiteTexture
        );
    }

    private void OnDisable()
    {
        UnhoverCurrentObject();
        DeselectObject();
    }
}
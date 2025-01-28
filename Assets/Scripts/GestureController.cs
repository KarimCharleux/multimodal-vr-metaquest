using UnityEngine;

public class GestureController : MonoBehaviour
{
    [Header("Selection Settings")]
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private float maxSelectionDistance = 10f;
    [SerializeField] private Material selectedObjectMaterial;
    [SerializeField] private Material hoveredObjectMaterial;
    
    [Header("Manipulation Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float objectMoveDistance = 5f;
    [SerializeField] private bool showDebugLogs = true;

    private Camera mainCamera;
    private GameObject selectedObject;
    private GameObject hoveredObject;
    private Material originalMaterial;
    private Material originalHoverMaterial;
    private bool isMovingObject = false;
    
    // Crosshair settings
    private bool showCrosshair = true;
    private float crosshairSize = 20f;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found!");
            enabled = false;
            return;
        }

        // Create hover material if not assigned
        if (hoveredObjectMaterial == null)
        {
            hoveredObjectMaterial = new Material(Shader.Find("Standard"));
            hoveredObjectMaterial.color = new Color(1f, 1f, 0f, 0.5f); // Semi-transparent yellow
        }
    }

    private void Update()
    {
        // Check for object under crosshair
        CheckHover();

        // Handle object selection and movement
        if (Input.GetMouseButtonDown(0))
        {
            // Use screen center for raycast since cursor is locked
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayer))
            {
                if (!isMovingObject)
                {
                    SelectObject(hit.collider.gameObject);
                    isMovingObject = true;
                }
            }
            else if (isMovingObject)
            {
                DeselectObject();
                isMovingObject = false;
            }
        }

        // Handle object movement
        if (isMovingObject && selectedObject != null)
        {
            Vector3 targetPosition = mainCamera.transform.position + 
                                   mainCamera.transform.forward * objectMoveDistance;
            
            selectedObject.transform.position = Vector3.Lerp(
                selectedObject.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }

        // Cancel movement with right click
        if (Input.GetMouseButtonDown(1) && isMovingObject)
        {
            DeselectObject();
            isMovingObject = false;
        }
    }

    private void CheckHover()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            // Don't hover the selected object
            if (hitObject != selectedObject)
            {
                if (hoveredObject != hitObject)
                {
                    // Unhover previous object
                    UnhoverCurrentObject();
                    
                    // Hover new object
                    hoveredObject = hitObject;
                    var renderer = hoveredObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalHoverMaterial = renderer.material;
                        renderer.material = hoveredObjectMaterial;
                    }
                }
            }
        }
        else
        {
            UnhoverCurrentObject();
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

    private void SelectObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            DeselectObject();
        }

        // Unhover the object if it's being selected
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
        DebugLog($"Selected object: {selectedObject.name}");
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
            DebugLog($"Deselected object: {selectedObject.name}");
            selectedObject = null;
        }
    }

    private void OnGUI()
    {
        if (showCrosshair)
        {
            float centerX = Screen.width / 2;
            float centerY = Screen.height / 2;
            
            GUI.color = hoveredObject != null ? Color.yellow : Color.white;
            GUI.DrawTexture(
                new Rect(centerX - crosshairSize/2, centerY - 1, crosshairSize, 2), 
                Texture2D.whiteTexture
            );
            GUI.DrawTexture(
                new Rect(centerX - 1, centerY - crosshairSize/2, 2, crosshairSize), 
                Texture2D.whiteTexture
            );
        }
    }

    private void OnDisable()
    {
        // Clean up when disabled
        UnhoverCurrentObject();
        DeselectObject();
    }

    private void DebugLog(string message)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[GestureController] {message}");
        }
    }
}
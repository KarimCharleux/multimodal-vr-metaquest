using UnityEngine;

public class MobileGestureController : MonoBehaviour
{
    [Header("Camera Control")]
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Selection Settings")]
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private float maxSelectionDistance = 10f;
    [SerializeField] private Material selectedObjectMaterial;
    [SerializeField] private Material hoveredObjectMaterial;

    private Camera mainCamera;
    private GameObject selectedObject;
    private GameObject hoveredObject;
    private Material originalMaterial;
    private Material originalHoverMaterial;
    private bool isMovingObject = false;
    private Vector2 touchStart;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found!");
            enabled = false;
            return;
        }

        // Initialize starting rotation
        Vector3 rotation = mainCamera.transform.rotation.eulerAngles;
        rotationY = rotation.y;
        rotationX = rotation.x;

        // Create hover material if not assigned
        if (hoveredObjectMaterial == null)
        {
            hoveredObjectMaterial = new Material(Shader.Find("Standard"));
            hoveredObjectMaterial.color = new Color(1f, 1f, 0f, 0.5f);
        }
    }

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        // No touch
        if (Input.touchCount == 0)
        {
            CheckHover(); // Check for objects in the center of screen
            return;
        }

        // Single touch - either camera rotation or object selection
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStart = touch.position;
                    // Check for object selection if touch just started
                    if (!isMovingObject)
                    {
                        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayer))
                        {
                            SelectObject(hit.collider.gameObject);
                            isMovingObject = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (isMovingObject)
                    {
                        // Move selected object
                        Vector3 targetPosition = mainCamera.transform.position + 
                                              mainCamera.transform.forward * 3f;
                        selectedObject.transform.position = Vector3.Lerp(
                            selectedObject.transform.position,
                            targetPosition,
                            moveSpeed * Time.deltaTime
                        );
                    }
                    else
                    {
                        // Rotate camera
                        rotationY += touch.deltaPosition.x * rotationSpeed;
                        rotationX -= touch.deltaPosition.y * rotationSpeed;
                        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
                        mainCamera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isMovingObject && Vector2.Distance(touch.position, touchStart) < 10f)
                    {
                        // If it was a tap while moving object, place the object
                        DeselectObject();
                        isMovingObject = false;
                    }
                    break;
            }
        }

        // Two finger touch - movement
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                // Calculate the average movement of both touches
                Vector2 deltaPosition = (touch0.deltaPosition + touch1.deltaPosition) / 2f;

                // Move in the direction the camera is facing
                Vector3 moveDirection = mainCamera.transform.forward * deltaPosition.y +
                                     mainCamera.transform.right * deltaPosition.x;
                moveDirection.y = 0; // Keep movement horizontal
                
                mainCamera.transform.position += moveDirection * moveSpeed * Time.deltaTime * 0.01f;
            }
        }
    }

    private void CheckHover()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxSelectionDistance, selectableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            
            if (hitObject != selectedObject && hitObject != hoveredObject)
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

    private void OnDisable()
    {
        UnhoverCurrentObject();
        DeselectObject();
    }
}
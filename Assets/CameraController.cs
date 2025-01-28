using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    [SerializeField] private float vitesseDeplacement = 5f;
    [SerializeField] private float vitesseRotation = 2f;
    [SerializeField] private float hauteurCamera = 1.8f;
    
    private float rotationX = 0f;
    private float rotationY = 0f;
    
    void Start()
    {
        // Verrouiller et cacher le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initialiser la position de la caméra
        Vector3 position = transform.position;
        position.y = hauteurCamera;
        transform.position = position;
    }
    
    void Update()
    {
        // Gestion de la rotation avec la souris
        rotationX -= Input.GetAxis("Mouse Y") * vitesseRotation;
        rotationY += Input.GetAxis("Mouse X") * vitesseRotation;
        
        // Limiter la rotation verticale
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        
        // Appliquer la rotation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        // Calculer la direction du mouvement
        Vector3 direction = Vector3.zero;
        
        // ZQSD pour le déplacement
        if (Input.GetKey(KeyCode.Z))
            direction += transform.forward;
        if (Input.GetKey(KeyCode.S))
            direction -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            direction += transform.right;
        if (Input.GetKey(KeyCode.Q))
            direction -= transform.right;
        
        // Normaliser et appliquer le mouvement
        if (direction != Vector3.zero)
        {
            direction.Normalize();
            transform.position += direction * vitesseDeplacement * Time.deltaTime;
            
            // Maintenir la hauteur constante
            Vector3 position = transform.position;
            position.y = hauteurCamera;
            transform.position = position;
        }
        
        // Déverrouiller la souris avec Échap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
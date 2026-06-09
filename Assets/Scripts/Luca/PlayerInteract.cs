using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float maxInteractDistance = 5f;
    [SerializeField] private LayerMask interactLayer;
    private IInteractable currentInteractable;
    
    public static event Action OnInteractAllowed, OnInteractNull;
    
    private void OnEnable()
    {
        InputManager.OnInteraction += HandleInteraction;
    }
    
    private void OnDisable()
    {
        InputManager.OnInteraction -= HandleInteraction;
    }
    
    private void Update()
    {
        CheckForInteractables();
    }
    
    private void CheckForInteractables()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                OnInteractAllowed?.Invoke();
                return;
            }
        }
        currentInteractable = null;
        OnInteractNull?.Invoke();
    }
    
    private void HandleInteraction()
    {
        currentInteractable?.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * maxInteractDistance);
    }
}

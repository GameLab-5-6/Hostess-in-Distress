using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text interactPrompt;
    [SerializeField] private Image Crosshair;

    private void Start()
    {
        interactPrompt.gameObject.SetActive(false);
        Crosshair.gameObject.SetActive(true);
    }
    
    private void OnEnable()
    {
        PlayerInteract.OnInteractAllowed += ShowInteract;
        PlayerInteract.OnInteractNull += HideInteract;
        GameManager.OnPause += HideCrosshair;
        GameManager.OnResume += ShowCrosshair;
    }

    private void OnDisable()
    {
        PlayerInteract.OnInteractAllowed -= ShowInteract;
        PlayerInteract.OnInteractNull -= HideInteract;
        GameManager.OnPause -= HideCrosshair;
        GameManager.OnResume -= ShowCrosshair;
    }

    private void HideCrosshair() => Crosshair.gameObject.SetActive(false);

    private void ShowCrosshair() => Crosshair.gameObject.SetActive(true);

    private void HideInteract() => interactPrompt.gameObject.SetActive(false);
    
    private void ShowInteract() => interactPrompt.gameObject.SetActive(true);
}

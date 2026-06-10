using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameManager gm;
    private PlayerInteract pi;
    
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text interactPrompt;
    [SerializeField] private TMP_Text eventPrompt;
    [SerializeField] private Image sanityBar;
    [SerializeField] private Image satisfactionBar;
    [SerializeField] private Image chargeBar;
    
    private void Awake()
    {
        gm = FindAnyObjectByType<GameManager>();
        pi = FindAnyObjectByType<PlayerInteract>();
    }
    
    private void Start()
    {
        gamePanel.gameObject.SetActive(true);
        interactPrompt.gameObject.SetActive(false);
        eventPrompt.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        GameManager.OnPause += HideGamePanel;
        GameManager.OnResume += ShowGamePanel;
    }

    private void OnDisable()
    {
        GameManager.OnPause -= HideGamePanel;
        GameManager.OnResume -= ShowGamePanel;
    }

    private void Update()
    {
        sanityBar.fillAmount = gm.currentSanity / gm.maxSanity;
        satisfactionBar.fillAmount = gm.currentSatisfaction / gm.maxSatisfaction;
        chargeBar.fillAmount = pi.chargeAmount / pi.chargeTime;
        
        if (pi.currentInteractable != null)
            interactPrompt.gameObject.SetActive(true);
        else
            interactPrompt.gameObject.SetActive(false);
        
        if (pi.currentEventable != null)
            eventPrompt.gameObject.SetActive(true);
        else
            eventPrompt.gameObject.SetActive(false);
    }

    private void HideGamePanel() => gamePanel.gameObject.SetActive(false);

    private void ShowGamePanel() => gamePanel.gameObject.SetActive(true);
}

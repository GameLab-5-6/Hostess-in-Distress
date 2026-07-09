using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameManager gm;
    private PlayerInteract pi;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private GameObject volumePanel;

    [SerializeField] private TMP_Text interactPrompt;
    [SerializeField] private float interactPromptTime = 3f;
    private float elapsedInteractTime;
    //[SerializeField] private TMP_Text eventPrompt;
    [SerializeField] private Image sanityBar;
    [SerializeField] private Image satisfactionBar;
    [SerializeField] private Image chargeBar;

    [SerializeField] private GameObject planeImage;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private float elapsedGameTime;

    private void Awake()
    {
        gm = FindAnyObjectByType<GameManager>();
        pi = FindAnyObjectByType<PlayerInteract>();
    }

    private void Start()
    {
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        interactPrompt.gameObject.SetActive(false);
        
        selectionPanel.gameObject.SetActive(true);
        volumePanel.gameObject.SetActive(false);
        //eventPrompt.gameObject.SetActive(false);
        
        elapsedInteractTime = interactPromptTime;
        elapsedGameTime = 0f;
    }

    private void OnEnable()
    {
        GameManager.OnPause += OpenPausePanel;
        GameManager.OnResume += HidePausePanel;
        GameManager.OnGameWin += OpenWinPanel;
        GameManager.OnGameLose += OpenLosePanel;
        
        BabyEvent.OnInteraction += ActivateInteractPrompt;
        MusicEvent.OnInteraction += ActivateInteractPrompt;
        DrinkEvent.OnInteraction += ActivateInteractPrompt;
        SpawnInteractable.OnFailedInteraction += ActivateInteractPrompt;
    }

    private void OnDisable()
    {
        GameManager.OnPause -= OpenPausePanel;
        GameManager.OnResume -= HidePausePanel;
        GameManager.OnGameWin -= OpenWinPanel;
        GameManager.OnGameLose -= OpenLosePanel;
        
        BabyEvent.OnInteraction -= ActivateInteractPrompt;
        MusicEvent.OnInteraction -= ActivateInteractPrompt;
        DrinkEvent.OnInteraction -= ActivateInteractPrompt;
        SpawnInteractable.OnFailedInteraction -= ActivateInteractPrompt;
    }

    private void Update()
    {
        sanityBar.fillAmount = gm.currentSanity / gm.maxSanity;
        satisfactionBar.fillAmount = gm.currentSatisfaction / gm.maxSatisfaction;
        chargeBar.fillAmount = pi.chargeAmount / pi.chargeTime;

        // if (pi.currentInteractable != null)
        //     interactPrompt.gameObject.SetActive(true);
        // else
        //     interactPrompt.gameObject.SetActive(false);

        if (elapsedInteractTime <= interactPromptTime)
        {
            elapsedInteractTime += Time.deltaTime;
            interactPrompt.gameObject.SetActive(true);
        }
        else
            interactPrompt.gameObject.SetActive(false);

        // if (pi.currentEventable != null)
        //     eventPrompt.gameObject.SetActive(true);
        // else
        //     eventPrompt.gameObject.SetActive(false);
        
        elapsedGameTime += Time.deltaTime;
        planeImage.transform.position = Vector3.Lerp(pointA.position, pointB.position, elapsedGameTime / gm.totalGameTime);
    }

    private void ActivateInteractPrompt(string text)
    {
        interactPrompt.text = text;
        elapsedInteractTime = 0f;
    }

    private void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        selectionPanel.SetActive(true);
        volumePanel.SetActive(false);
        
        gamePanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        selectionPanel.SetActive(false);
        volumePanel.SetActive(true);
    }

    public void ReturnToSelectionPanel()
    {
        selectionPanel.SetActive(true);
        volumePanel.SetActive(false);
    }
    
    private void HidePausePanel()
    {
        pausePanel.SetActive(false);
        selectionPanel.SetActive(true);
        volumePanel.SetActive(false);
        
        gamePanel.SetActive(true);
    }

    private void OpenWinPanel()
    {
        gamePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(true);
        InputManager.OnPauseAllowed?.Invoke();
    }

    private void OpenLosePanel()
    {
        gamePanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(true);
        InputManager.OnPauseAllowed?.Invoke();
    }
}

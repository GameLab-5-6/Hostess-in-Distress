using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject previousPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject page1;
    [SerializeField] private GameObject page2;

    private void Start()
    {
        tutorialPanel.SetActive(false);
    }
    
    public void OpenTutorialPanel()
    {
        previousPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        page1.SetActive(true);
        page2.SetActive(false);
    }
    
    public void TurnToPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }

    public void TurnToPage1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }
}

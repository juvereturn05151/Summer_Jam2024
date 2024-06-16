using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIManager : MonoBehaviour
{
    public GameObject[] tutorialPages; // Array of tutorial pages

    [SerializeField]
    private GameObject previousButton;

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private GameObject startButton;

    private int currentPageIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShowPage(currentPageIndex); // Show the first page when the tutorial starts
    }

    // Update is called once per frame
    void Update()
    {
        // You can also implement keyboard shortcuts or buttons for navigation
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowPreviousPage();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ShowNextPage();
        }
    }

    // Show the specified page and hide others
    void ShowPage(int pageIndex)
    {
        SoundManager.Instance.PlayOneShot("SFX_Click");

        for (int i = 0; i < tutorialPages.Length; i++)
        {
            if (i == pageIndex)
            {
                tutorialPages[i].SetActive(true);
            }
            else
            {
                tutorialPages[i].SetActive(false);
            }
        }

        previousButton.SetActive(pageIndex > 0);
        nextButton.SetActive(pageIndex < tutorialPages.Length - 1);
        startButton.SetActive(pageIndex == tutorialPages.Length - 1);
    }

    // Show the previous page
    public void ShowPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }

    // Show the next page
    public void ShowNextPage()
    {
        if (currentPageIndex < tutorialPages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    public void Play()
    {
        FadingUI.Instance.StartFadeIn();
        FadingUI.OnStopFading += LoadScene;
    }

    private void LoadScene() 
    {
        SceneManager.LoadScene("QiqiRealGameplay");
    }
}

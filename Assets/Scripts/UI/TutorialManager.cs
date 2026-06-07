using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    int page = 1;
    const int maxPage = 8;
    public Text pageT;
    [SerializeField] GameObject[] pages;
    [SerializeField] GameObject prevButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject mapImage;
    [SerializeField] Text pageNumText;
    bool isMapOn = false;

    private void Start() 
    {
        initTutorial();
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(isMapOn)
            {
                mapImage.GetComponent<WindowAnimation>().OffAnimUI();
                isMapOn = false;
            }
        }
    }

    private void initTutorial()
    {
        page = 1;
        prevButton.SetActive(false);
        for(int i = 0; i < pages.Length; i++)
        {
            if(i != 0) pages[i].SetActive(false);
            else pages[i].SetActive(true);
        }
        pageNumText.text = "1";
    }

    public void prevPageButton()
    {
        if(page == 2) 
        {
            prevButton.SetActive(false);
        }
        nextButton.SetActive(true);

        pages[page - 1].SetActive(false);
        pages[page - 2].SetActive(true);

        pageNumText.text = (page - 1).ToString();

        page--;
    }

    public void nextPageButton()
    {
        if(page == maxPage - 1)
        {
            nextButton.SetActive(false);
        } 
        prevButton.SetActive(true);
        
        pages[page - 1].SetActive(false);
        pages[page].SetActive(true);
        
        pageNumText.text = (page + 1).ToString();

        page++;
    }

    public void mission4Click()
    {
        mapImage.GetComponent<WindowAnimation>().OnAnimUI();
        isMapOn = true;
    }
    
}

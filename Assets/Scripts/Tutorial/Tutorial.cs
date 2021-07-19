using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject holdera;
    public GameObject[] tutorialSlides;
    public bool tutorialStarted = false;
    public int tutorialSlide = 1;
    public bool disableBuilding = false;
    public bool disableMoving = false;
    public bool disableMenus = false;
    public bool spaceToContinue = false;

    public void enableTutorial()
    {
        holdera.SetActive(true);
        tutorialSlides[0].SetActive(true);
        tutorialStarted = true;
        disableBuilding = true;
        disableMoving = true;
        disableMenus = true;
        spaceToContinue = true;
    }

    private void Start()
    {
        holdera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialStarted && Input.GetKeyDown(KeyCode.Space) && spaceToContinue) nextSlide();
    }

    public void addSlides(GameObject[] slides)
    {
        tutorialSlides = slides;
    }

    public void nextSlide()
    {
        tutorialSlides[tutorialSlide-1].SetActive(false);
        tutorialSlide++;

        if (tutorialSlide == 16)
        {
            tutorialStarted = false;
            disableBuilding = false;
            disableMoving = false;
            disableMenus = false;
            spaceToContinue = false;
            enabled = false;
            return;
        }

        tutorialSlides[tutorialSlide-1].SetActive(true);

        switch(tutorialSlide)
        {
            case 3:
                disableBuilding = false;
                disableMoving = false;
                spaceToContinue = false;
                break;
            case 4:
                disableBuilding = true;
                disableMoving = false;
                disableMenus = false;
                break;
            case 7:
                disableBuilding = false;
                break;
            case 8:
                disableBuilding = true;
                break;
            case 10:
                disableBuilding = false;
                break;
            case 11:
                disableBuilding = true;
                spaceToContinue = true;
                break;
            case 13:
                disableBuilding = false;
                spaceToContinue = false;
                break;
            case 14:
                disableBuilding = true;
                spaceToContinue = true;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] tutorialSlides;
    public bool tutorialStarted = false;
    public int tutorialSlide = 1;
    public bool disableBuilding = false;
    public bool disableMoving = false;
    public bool disableMenus = false;
    public bool spaceToContinue = false;
    
    public bool freeBuilding = false;

    public void enableTutorial()
    {
        freeBuilding = false;

        tutorialStarted = true;
        disableBuilding = true;
        disableMoving = true;
        disableMenus = true;
        spaceToContinue = true;
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
            freeBuilding = false;
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
                freeBuilding = true;
                break;
            case 4:
                freeBuilding = false;
                disableBuilding = true;
                disableMoving = false;
                disableMenus = false;
                break;
            case 7:
                disableBuilding = false;
                freeBuilding = true;
                break;
            case 8:
                disableBuilding = true;
                freeBuilding = false;
                break;
            case 10:
                disableBuilding = false;
                freeBuilding = true;
                break;
            case 11:
                disableBuilding = true;
                freeBuilding = false;
                spaceToContinue = true;
                break;
            case 13:
                disableBuilding = false;
                freeBuilding = true;
                spaceToContinue = false;
                break;
            case 14:
                disableBuilding = true;
                freeBuilding = false;
                spaceToContinue = true;
                break;
        }
    }
}

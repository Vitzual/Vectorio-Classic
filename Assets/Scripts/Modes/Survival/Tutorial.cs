using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // Tutorial thing
    [System.Serializable]
    public class Slide
    {
        public enum Task
        {
            Space,
            Place,
            Collect
        }

        public GameObject obj;
        public Task task;
        public Building optionalEntity;

        public void Enable()
        {
            obj.SetActive(true);
        }

        public void Disable()
        {
            obj.SetActive(false);
        }
    }

    // Tutorial variables
    public AudioPlayer audioPlayer;
    public GameObject tutorialObject;
    public Slide[] tutorialSlides;
    public bool tutorialStarted = false;
    public int tutorialSlide = 0;

    // Enables the tutorial
    public void EnableTutorial()
    {
        // Enable tutorial variables
        tutorialObject.SetActive(true);
        tutorialStarted = true;
        tutorialSlide = 0;
        tutorialSlides[tutorialSlide].Enable();

        // Setup events
        InputEvents.active.onSpacePressed += SpacePressed;
        Events.active.onBuildingPlaced += BuildingPlaced;
        Events.active.onCollectorHarvested += GoldCollected;
    }

    // Disables the active tutorial
    public void DisableTutorial()
    {
        // Remove events
        InputEvents.active.onSpacePressed -= SpacePressed;
        Events.active.onBuildingPlaced -= BuildingPlaced;
        Events.active.onCollectorHarvested -= GoldCollected;

        // Disable tutorial
        tutorialObject.SetActive(false);
        tutorialSlide = 0;
        tutorialStarted = false;
        enabled = false;
    }
    
    // Disable previous slide and enable next
    // If no next slide, end tutorial sequence 
    public void NextSlide()
    {
        tutorialSlides[tutorialSlide].Disable();
        tutorialSlide += 1;

        if (tutorialSlide < tutorialSlides.Length)
        {
            tutorialSlides[tutorialSlide].Enable();
            audioPlayer.PlayAudio();
        }
        else DisableTutorial();
    }

    // On space pressed, check tutorial and move to next slide if passed
    public void SpacePressed()
    {
        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Space) NextSlide();
    }

    // On building placed, check tutorial and move to next slide if passed
    public void BuildingPlaced(BaseTile building)
    {
        Debug.Log("[TUTORIAL] Received building pressed event");

        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Place &&
            tutorialSlides[tutorialSlide].optionalEntity == building.buildable.building) NextSlide();
    }

    // On gold collected, check tutorial and move to next slide if passed
    public void GoldCollected(Resource.CurrencyType type, int amount)
    {
        Debug.Log("[TUTORIAL] Received gold collected event");

        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Collect) NextSlide();
    }
}

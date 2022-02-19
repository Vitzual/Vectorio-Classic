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
            Collect,
            Metadata
        }

        public GameObject obj;
        public Task task;
        public Building optionalEntity;
        public bool hideTutorial;

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
    public static bool tutorialStarted = false;
    public static Building tutorialBuilding;
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
        Events.active.onMetadataChanged += MetadataChanged;
    }

    // Disables the active tutorial
    public void DisableTutorial()
    {
        // Disable tutorial
        tutorialObject.SetActive(false);
        tutorialSlide = 0;
        tutorialStarted = false;
        tutorialBuilding = null;
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
            tutorialBuilding = tutorialSlides[tutorialSlide].optionalEntity;
            audioPlayer.PlayAudio();
        }
        else DisableTutorial();
    }

    // On building unlocked
    public void MetadataChanged(int runtimeID, int metadata)
    {
        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Metadata) NextSlide();
    }

    // On space pressed, check tutorial and move to next slide if passed
    public void SpacePressed()
    {
        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Space) NextSlide();
    }

    // On building placed, check tutorial and move to next slide if passed
    public void BuildingPlaced(BaseTile building)
    {
        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Place &&
            tutorialSlides[tutorialSlide].optionalEntity == building.buildable.building) NextSlide();
    }

    // On gold collected, check tutorial and move to next slide if passed
    public void GoldCollected(Resource.Type type, int amount)
    {
        if (tutorialStarted && tutorialSlides[tutorialSlide].task == Slide.Task.Collect) NextSlide();
    }
}

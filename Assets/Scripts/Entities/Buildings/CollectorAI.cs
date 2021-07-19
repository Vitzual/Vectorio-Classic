using System.Collections;
using UnityEngine;

public class CollectorAI: TileClass
{
    // Declare local object variables
    public int collectorType;
    public int collected;
    public bool enhanced;
    private bool isFirstAnim = true;
    private bool isOffset = false;
    public bool isMenu = false;
    public AnimateThenStop animator;
    private DroneManager droneManager;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        animator = GetComponent<AnimateThenStop>();
        if (isMenu) return;

        droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        droneManager.updateResourceDrones(transform);

        switch (collectorType) {
            case 1:
                if (Research.research_gold_time >= 1 && !isOffset)
                    InvokeRepeating("IncreaseGold", 0f, Research.research_gold_time);
                else if (!isOffset) InvokeRepeating("IncreaseGold", 0f, 2f);
                return;
            case 2:
                if (Research.research_essence_time >= 1 && !isOffset)
                    InvokeRepeating("IncreaseEssence", 0f, Research.research_essence_time);
                else if (!isOffset) InvokeRepeating("IncreaseEssence", 0f, 2f);
                return;
            case 3:
                if (Research.research_iridium_time >= 1 && !isOffset)
                    InvokeRepeating("IncreaseIridium", 0f, Research.research_iridium_time);
                else if (!isOffset) InvokeRepeating("IncreaseIridium", 0f, 2f);
                return;
        }
    }

    public IEnumerator OffsetStart()
    {
        isOffset = true;
        isFirstAnim = false;

        switch (collectorType)
        {
            case 1:
                CancelInvoke("IncreaseGold");
                if (Research.research_gold_time >= 0.5f)
                {
                    yield return new WaitForSeconds(Random.Range(0f, Research.research_gold_time));
                    if (this != null) InvokeRepeating("IncreaseGold", 0f, Research.research_gold_time);
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(0f, 5f));
                    if (this != null) InvokeRepeating("IncreaseGold", 0f, 2f);
                }
                break;
            case 2:
                CancelInvoke("IncreaseEssence");
                if (Research.research_essence_time >= 0.5f)
                {
                    yield return new WaitForSeconds(Random.Range(0f, Research.research_essence_time));
                    if (this != null) InvokeRepeating("IncreaseEssence", 0f, Research.research_essence_time);
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(0f, 8f));
                    if (this != null) InvokeRepeating("IncreaseEssence", 0f, 8f);
                }
                break;
            case 3:
                CancelInvoke("IncreaseIridium");
                if (Research.research_iridium_time >= 0.5f)
                {
                    yield return new WaitForSeconds(Random.Range(0f, Research.research_iridium_time));
                    if (this != null) InvokeRepeating("IncreaseIridium", 0f, Research.research_iridium_time);
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(0f, 15f));
                    if (this != null) InvokeRepeating("IncreaseIridium", 0f, 15f);
                }
                break;
        }

    }

    // Send gold
    private void IncreaseGold()
    {
        if (!isFirstAnim && collected <= 1000)
            collected += Research.research_gold_yield;
        else isFirstAnim = false;
    }

    // Send essence
    private void IncreaseEssence()
    {
        if (!isFirstAnim && collected <= 1000)
            collected += Research.research_essence_yield;
        else isFirstAnim = false;
    }

    // Send iridium
    private void IncreaseIridium()
    {
        if (!isFirstAnim && collected <= 1000)
            collected += Research.research_iridium_yield;
        else isFirstAnim = false;
    }

    public int GrabResources()
    {
        // Set animation
        animator.resetAnim();
        animator.animEnabled = true;
        animator.enabled = true;

        // Set values
        int holder = collected;
        collected = 0;
        return holder;
    }

    // Enhance collector
    public void enhanceCollector()
    {
        enhanced = true;
    }

    // Deenhance collector
    public void deenhanceCollector()
    {
        enhanced = false;
    }

    public Transform getPosition()
    {
        return transform;
    }
}

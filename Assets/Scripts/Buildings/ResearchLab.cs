using System.Collections.Generic;
using UnityEngine;

public class ResearchLab : BaseTile
{
    public SpriteRenderer boostIcon;
    public ResearchTech researchTech;
    public AudioClip boomSound;
    public int strikes = 3;

    public void Start()
    {
        InvokeRepeating("UpdateResources", 0, 1);
    }

    public override void OnClick()
    {
        Events.active.LabClicked(this);
    }

    public void ApplyResearch(ResearchTech type, bool overrideCost = false)
    {
        if (!overrideCost)
            foreach (Cost cost in type.cost)
                if (cost.add) Resource.active.Add(cost.resource, cost.amount, false);

        researchTech = type;
        boostIcon.gameObject.SetActive(true);
        boostIcon.sprite = type.icon;
        Research.ApplyResearch(type, false);

        metadata = type.metadataID;
    }

    public void CancelResearch()
    {
        if (researchTech != null)
        {
            Research.ApplyResearch(researchTech, true);

            foreach (Cost cost in researchTech.cost)
                if (cost.add) Resource.active.Remove(cost.resource, cost.amount, false);

            boostIcon.gameObject.SetActive(false);
            researchTech = null;
            metadata = -1;
        }
    }

    public void UpdateResources()
    {
        if (researchTech != null)
        {
            foreach (Cost cost in researchTech.cost)
            {
                if (!cost.add)
                {
                    if (Resource.active.currencies[cost.resource].amount >= cost.amount)
                    {
                        Resource.active.Remove(cost.resource, cost.amount, true);
                    }
                    else if (strikes == 0)
                    {
                        Debug.Log("Lab no longer has resources to continue operating, stopping");
                        Debug.Log("Failed on " + cost.resource);
                        Events.active.LabDestroyed(this);

                        AudioSource.PlayClipAtPoint(boomSound, transform.position, 0.5f);
                        if (Research.techs.ContainsKey(researchTech))
                            Research.techs[researchTech].totalBooms += 1;
                        if (ResearchUI.active != null) ResearchUI.CloseMenu();
                        DestroyEntity();
                        return;
                    }
                    else
                    {
                        strikes -= 1;
                        return;
                    }
                }
            }
        }

        strikes = 3;
    }

    public override void ApplyMetadata(int data)
    {
        foreach (KeyValuePair<string, ResearchTech> tech in ScriptableLoader.researchTechs)
            if (tech.Value.metadataID == data) ApplyResearch(tech.Value, true);
    }

    public override void DestroyEntity()
    {
        CancelResearch();
        base.DestroyEntity();
    }
}

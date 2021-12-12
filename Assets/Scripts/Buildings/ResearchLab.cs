using System.Collections.Generic;
using UnityEngine;

public class ResearchLab : BaseTile
{
    public SpriteRenderer boostIcon;
    public ResearchTech researchTech;
    public AudioClip boomSound;
    public int strikes = 5;
    
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
        if (researchTech != null)
            CancelResearch();

        if (!overrideCost)
            Resource.active.ApplyOutputsOnly(type.cost.ToArray());

        researchTech = type;
        boostIcon.gameObject.SetActive(true);
        boostIcon.sprite = type.icon;
        Research.ApplyResearch(type, false);

        ResearchUI.active.researchButtons[type].UpdateButton();
        ResearchUI.active.SetPanel(type);

        metadata = type.metadataID;
    }

    public void CancelResearch()
    {
        if (researchTech != null)
        {
            Research.ApplyResearch(researchTech, true);
            Resource.active.RefundOutputsOnly(researchTech.cost.ToArray());

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
                if (!Resource.active.currencies[cost.type].output)
                {
                    if (Resource.active.currencies[cost.type].amount >= cost.amount)
                    {
                        Resource.active.Apply(cost.type, -cost.amount, true);
                    }
                    else if (strikes == 0)
                    {
                        Debug.Log("Lab no longer has resources to continue operating, stopping");
                        Debug.Log("Failed on " + cost.type);
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

        strikes = 5;
    }

    public override void ApplyMetadata(int data)
    {
        foreach (KeyValuePair<string, ResearchTech> tech in ScriptableLoader.researchTechs)
            if (tech.Value.metadataID == data) ApplyResearch(tech.Value, true);
        base.ApplyMetadata(data);
    }

    public override void DestroyEntity()
    {
        CancelResearch();
        base.DestroyEntity();
    }
}

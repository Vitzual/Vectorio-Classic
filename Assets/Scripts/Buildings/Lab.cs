using UnityEngine;

public class Lab : BaseTile
{
    public bool working = false;
    public SpriteRenderer boostIcon;
    public ResearchBoost activeBoost;

    public void Start()
    {
        InvokeRepeating("UpdateResources", 0, 1);
    }

    public override void OnClick()
    {
        Events.active.LabClicked(this);
    }

    public void ApplyResearch(ResearchBoost type)
    {
        if (activeBoost == type) CancelResearch();
        else
        {
            foreach (Cost cost in type.cost)
            {
                if (cost.storage)
                {
                    if (cost.add) Resource.active.Add(cost.resource, cost.amount, false);
                    else Resource.active.Remove(cost.resource, cost.amount, false);
                }
            }

            working = true;
            activeBoost = type;
            Research.ApplyResearch(type.type, type.amount, type.currency);
        }
    }

    public void CancelResearch()
    {
        if (activeBoost != null)
        {
            if (working) Research.ApplyResearch(activeBoost.type, -activeBoost.amount, activeBoost.currency);

            foreach (Cost cost in activeBoost.cost)
            {
                if (cost.storage)
                {
                    if (cost.add) Resource.active.Remove(cost.resource, cost.amount, false);
                    else Resource.active.Add(cost.resource, cost.amount, false);
                }
            }

            activeBoost = null;
        }
    }

    public void UpdateResources()
    {
        if (activeBoost != null)
        {
            foreach (Cost cost in activeBoost.cost)
            {
                if (!cost.storage)
                {
                    if (Resource.active.currencies[cost.resource].amount >= cost.amount) 
                    { 
                        if (cost.add) Resource.active.Add(cost.resource, cost.amount, false);
                        else Resource.active.Remove(cost.resource, cost.amount, false);
                    }
                    else if (working)
                    {
                        working = false;
                        Debug.Log("Lab no longer has reasources to continue operating, stopping");
                        Research.ApplyResearch(activeBoost.type, -activeBoost.amount, activeBoost.currency);
                        return;
                    }
                }
            }

            if (!working)
            {
                Debug.Log("Lab gained resources to continue operating");
                Research.ApplyResearch(activeBoost.type, activeBoost.amount, activeBoost.currency);
                working = true;
            }
        }
    }

    public override void DestroyEntity()
    {
        CancelResearch();
        base.DestroyEntity();
    }
}

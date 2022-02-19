using System.Collections.Generic;
using UnityEngine;

// Still needs to be refactored

public class Research : MonoBehaviour
{
    // Class
    [System.Serializable]
    public class Tech
    {
        public int totalLabs = 0;
        public int totalBooms = 0;
        public float totalEffect = 0;

        public Dictionary<Resource.Type, float> costs =
            new Dictionary<Resource.Type, float>();
    }

    // Create new list of techs
    public static Dictionary<ResearchTech, Tech> techs = 
              new Dictionary<ResearchTech, Tech>();

    // Active labs in scene
    public static List<ResearchLab> activeLabs = new List<ResearchLab>();

    // Turret variables
    public static float damageBoost = 1;
    public static float healthBoost = 1;
    public static float wallBoost = 1;
    public static int pierceBoost = 0;
    public static int bulletBoost = 0;
    public static float firerateBoost = 1;

    // Resource boosts
    public class ResourceBoost
    {
        public float extractionRate;
        public int extractionYield;
        public int storageAmount;
    }
    public static Dictionary<Resource.Type, ResourceBoost> resource = 
              new Dictionary<Resource.Type, ResourceBoost>();

    // Drone research variables
    public static int drone_tile_coverage = 5;
    public static float drone_deployment_speed = 3f;
    public static float drone_move_speed = 1f;

    // Reset research values
    public static void ResetResearch()
    {
        // Reset dictionaries
        resource = new Dictionary<Resource.Type, ResourceBoost>();
        techs = new Dictionary<ResearchTech, Tech>();
        activeLabs = new List<ResearchLab>();

        // Reset research values
        damageBoost = 1;
        healthBoost = 1;
        wallBoost = 1;
        pierceBoost = 0;
        bulletBoost = 0;
        firerateBoost = 1;
        drone_tile_coverage = 5;
        drone_deployment_speed = 3f;
        drone_move_speed = 1f;
    }

    // Currency get variables (I hate this, and will redo it)
    public static void GenerateBoost(Resource.Type type, float defaultRate, int defaultYield, int defaultStorage)
    {
        ResourceBoost newResource = new ResourceBoost();
        newResource.extractionRate = defaultRate;
        newResource.extractionYield = defaultYield;
        newResource.storageAmount = defaultStorage;
        resource.Add(type, newResource);
    }

    // Apply research
    // THIS IS GONNA BE REDONE
    public static void ApplyResearch(ResearchTech tech, bool revoke = true)
    {
        Debug.Log("Applying research " + tech.name + " with revoke set to " + revoke);

        float amount = tech.amount;
        if (revoke) amount = -amount;

        switch(tech.type)
        {
            case ResearchTypeEnum.DamageBoost:
                damageBoost += amount;
                break;
            case ResearchTypeEnum.HealthBoost:
                healthBoost += amount;
                break;
            case ResearchTypeEnum.WallBoost:
                wallBoost += amount;
                break;
            case ResearchTypeEnum.PierceBoost:
                if (revoke) pierceBoost -= 1;
                else pierceBoost += 1;
                break;
            case ResearchTypeEnum.BulletBoost:
                if (revoke) bulletBoost -= 1;
                else bulletBoost += 1;
                break;
            case ResearchTypeEnum.FirerateBoost:
                firerateBoost += amount;
                break;
            case ResearchTypeEnum.DroneSpeed:
                drone_move_speed += amount;
                break;
            case ResearchTypeEnum.ExtractionRate:
                resource[tech.currency].extractionRate -= amount;
                if (resource[tech.currency].extractionRate <= 0.2f)
                    resource[tech.currency].extractionRate = 0.2f;
                break;
            case ResearchTypeEnum.ExtractionYield:
                resource[tech.currency].extractionYield += (int)amount;
                break;
            case ResearchTypeEnum.StorageAmount:
                resource[tech.currency].storageAmount += (int)amount;
                break;
        }

        if (techs.ContainsKey(tech)) 
        {
            if (revoke)
            {
                foreach (Cost cost in tech.cost)
                    if (techs[tech].costs.ContainsKey(cost.type))
                        techs[tech].costs[cost.type] -= cost.amount;

                techs[tech].totalEffect -= tech.amount;
                techs[tech].totalLabs -= 1;
            }
            else
            {
                foreach (Cost cost in tech.cost)
                {
                    if (techs[tech].costs.ContainsKey(cost.type))
                        techs[tech].costs[cost.type] += cost.amount;
                    else techs[tech].costs.Add(cost.type, cost.amount);
                }
                techs[tech].totalEffect += tech.amount;
                techs[tech].totalLabs += 1;
            }
        }
        else
        {
            if (!revoke)
            {
                techs.Add(tech, new Tech());

                foreach (Cost cost in tech.cost)
                    techs[tech].costs.Add(cost.type, cost.amount);

                techs[tech].totalEffect = tech.amount;
                techs[tech].totalLabs = 1;
            }
        }
    }

    public static float GetAmount(ResearchTech tech)
    {
        switch (tech.type)
        {
            case ResearchTypeEnum.DamageBoost:
                return damageBoost;
            case ResearchTypeEnum.HealthBoost:
                return healthBoost;
            case ResearchTypeEnum.WallBoost:
                return wallBoost;
            case ResearchTypeEnum.PierceBoost:
                return pierceBoost;
            case ResearchTypeEnum.BulletBoost:
                return bulletBoost;
            case ResearchTypeEnum.FirerateBoost:
                return firerateBoost;
            case ResearchTypeEnum.DroneSpeed:
                return drone_move_speed;
            case ResearchTypeEnum.ExtractionRate:
                return resource[tech.currency].extractionRate;
            case ResearchTypeEnum.ExtractionYield:
                return resource[tech.currency].extractionRate;
            case ResearchTypeEnum.StorageAmount:
                return resource[tech.currency].extractionRate;
            default:
                return -1;
        }
    }
}

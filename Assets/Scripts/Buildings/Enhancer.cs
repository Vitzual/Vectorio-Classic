using UnityEngine;
using UnityEngine.SceneManagement;

public class Enhancer : BaseTile
{
    public override void Setup()
    {

        base.Setup();
    }

    /*
    // Kill defense
    public override void UpdateEnhancer()
    {
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Building"));
        for (int i = 0; i < colliders.Length; i++)
        {
            try
            {
                if (colliders[i].name.Contains("Collector"))
                    colliders[i].GetComponent<CollectorAI>().deenhanceCollector();
            }
            catch { continue; }
        }

        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.removeBuilding(transform);
        GameObject.Find("Spawner").GetComponent<Spawner>().decreaseHeat(heat);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    */
}

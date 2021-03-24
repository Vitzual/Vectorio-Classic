using UnityEngine;

public class EssenceAI: TileClass
{
    // Declare local object variables
    public int amount;
    public bool enhanced;
    private Survival SRVSC;

    // Popup variables
    public Transform popup;
    public ResourcePopup rPopup;
    public bool animPlaying = false;
    public bool isFirstAnim = true;

    public float sizeTracker = 1f;
    public bool sizeGrowing = true;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        InvokeRepeating("SendEssence", 0f, 8f);
        popup = Instantiate(popup, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        rPopup = popup.GetComponent<ResourcePopup>();
        popup.parent = SRVSC.UI.IngameCanvas.transform;
        rPopup.SetPopup(new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z));
    }

    private void Update()
    {
        if (animPlaying)
        {
            transform.localScale = new Vector2(sizeTracker, sizeTracker);
            if (sizeGrowing)
            {
                sizeTracker += 0.02f;
                if (sizeTracker >= 1.1f)
                    sizeGrowing = false;
            }
            else
            {
                sizeTracker -= 0.01f;
                if (sizeTracker <= 1.0f)
                {
                    animPlaying = false;
                    sizeGrowing = true;
                    sizeTracker = 1f;
                }
            }
        }
    }

    // Send gold
    private void SendEssence()
    {
        if (!isFirstAnim)
        {
            int add = amount + Research.bonus_gold;
            if (enhanced) add *= 4;
            SRVSC.AddEssence(add);
            rPopup.ResetPopup("+ " + add);
            animPlaying = true;
        }
        else isFirstAnim = false;
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
        sizeTracker = 1f;
        transform.localScale = new Vector2(1, 1);
        sizeGrowing = false;
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

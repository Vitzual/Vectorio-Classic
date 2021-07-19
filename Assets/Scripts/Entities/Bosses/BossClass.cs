using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossClass : EnemyClass
{
    public string bossTitle; // name is reserved #pogchamp
    public int bossID;
    public GameObject bossBar;

    // Start is called before the first frame update
    private new void Start()
    {
        bossBar = GameObject.Find("Survival").GetComponent<Interface>().GetBossBar(bossID);
        bossBar.SetActive(true);
        bossBar.GetComponent<ProgressBar>().maxValue = health;
        bossBar.GetComponent<ProgressBar>().currentPercent = health;
        bossBar.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = bossTitle;
        base.Start();
    }

    public override void DamageEntity(int dmgRecieved)
    {
        base.DamageEntity(dmgRecieved);
        bossBar.GetComponent<ProgressBar>().currentPercent = health;
    }

    public override void KillEntity()
    {
        bossBar.SetActive(false);
        base.KillEntity();
    }

}

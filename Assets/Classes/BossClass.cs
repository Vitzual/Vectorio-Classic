using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossClass : EnemyClass
{
    public string bossTitle; // name is reserved #pogchamp

    public GameObject bossBar;

    // Start is called before the first frame update
    private new void Start()
    {
        bossBar = GameObject.Find("Survival").GetComponent<Interface>().GetBossBar();
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

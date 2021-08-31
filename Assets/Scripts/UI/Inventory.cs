using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject buildable;

    public void CreateDefense()
    {
        GameObject newItem = Instantiate(buildable, new Vector3(0, 0, 0), Quaternion.identity);
        newItem.transform.SetParent(Unlocks[i].ParentObj);
        newItem.transform.localScale = new Vector3(1, 1, 1);
        newItem.transform.name = Unlocks[i].Building.name;
        Unlocks[i].ChildObj = newItem.transform;
        Unlocks[i].InvIcon = Unlocks[i].ChildObj.Find("Image").GetComponent<Image>();
        Unlocks[i].InvName = Unlocks[i].ChildObj.Find("Name").GetComponent<TextMeshProUGUI>();
        Unlocks[i].InvButton = Unlocks[i].ChildObj.Find("Button").GetComponent<Button>();
        Unlocks[i].Progress = Unlocks[i].ChildObj.Find("Progress");
        Unlocks[i].ProgressBar = Unlocks[i].Progress.GetComponent<ProgressBar>();
        Unlocks[i].ProgressDesc = Unlocks[i].Progress.Find("Goal").GetComponent<TextMeshProUGUI>();
        Unlocks[i].ProgressText = Unlocks[i].Progress.Find("Progress").GetComponent<TextMeshProUGUI>();
        Unlocks[i].ProgressIcon = Unlocks[i].Progress.Find("Icon").GetComponent<Image>();
    }
}

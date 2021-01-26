using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Catalog : MonoBehaviour
{
    public GameObject BuildingCatalog;
    public GameObject EnemyCatalog;
    public GameObject[] DiscoveredEnemies;
    public GameObject[] DiscoveredBuildings;
    public int enemyIndex = 0;
    public int buildingIndex = 0;

    public void SwitchBuildingsCatalog(int a)
    {
        buildingIndex += a;
        if (buildingIndex < 0 || buildingIndex > DiscoveredBuildings.Length) { buildingIndex -= a; return; }
        if(GameObject.Find("Survival").GetComponent<Technology>().checkIfBuildingUnlocked(DiscoveredBuildings[buildingIndex]))
        {
            BuildingCatalog.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].name;
            BuildingCatalog.transform.Find("Image 1").GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/" + DiscoveredBuildings[buildingIndex].name + " 1");
            BuildingCatalog.transform.Find("Image 2").GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/" + DiscoveredBuildings[buildingIndex].name + " 2");
            BuildingCatalog.transform.Find("Image 3").GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/" + DiscoveredBuildings[buildingIndex].name + " 3");
            //BuildingCatalog.transform.Find("Title 1").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].GetComponent<TileClass>().CatalogTitle1;
            //BuildingCatalog.transform.Find("Title 2").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].GetComponent<TileClass>().CatalogTitle2;
            //BuildingCatalog.transform.Find("Title 3").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].GetComponent<TileClass>().CatalogTitle3;
            //BuildingCatalog.transform.Find("Description 1").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].GetComponent<TileClass>().CatalogDesc1;
            //BuildingCatalog.transform.Find("Description 2").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].GetComponent<TileClass>().CatalogDesc2;
            //BuildingCatalog.transform.Find("Description 3").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[buildingIndex].GetComponent<TileClass>().CatalogDesc3;
        } 
        else
        {
            buildingIndex -= a;
        }
    }

    public void SwitchEnemiesCatalog(int a)
    {
        enemyIndex += a;
        if (enemyIndex < 0 || enemyIndex > DiscoveredBuildings.Length) { enemyIndex -= a; return; }
        if (GameObject.Find("Survival").GetComponent<Technology>().checkIfBuildingUnlocked(DiscoveredBuildings[enemyIndex]))
        {
            BuildingCatalog.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].name;
            BuildingCatalog.transform.Find("Image 1").GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/" + DiscoveredBuildings[enemyIndex].name + " 1");
            BuildingCatalog.transform.Find("Image 2").GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/" + DiscoveredBuildings[enemyIndex].name + " 2");
            BuildingCatalog.transform.Find("Image 3").GetComponent<Image>().sprite = Resources.Load<Sprite>("Pictures/" + DiscoveredBuildings[enemyIndex].name + " 3");
            //BuildingCatalog.transform.Find("Title 1").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].GetComponent<EnemyClass>().CatalogTitle1;
            //BuildingCatalog.transform.Find("Title 2").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].GetComponent<EnemyClass>().CatalogTitle2;
            //BuildingCatalog.transform.Find("Title 3").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].GetComponent<EnemyClass>().CatalogTitle3;
            //BuildingCatalog.transform.Find("Description 1").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].GetComponent<EnemyClass>().CatalogDesc1;
            //BuildingCatalog.transform.Find("Description 2").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].GetComponent<EnemyClass>().CatalogDesc2;
            //BuildingCatalog.transform.Find("Description 3").GetComponent<TextMeshProUGUI>().text = DiscoveredBuildings[enemyIndex].GetComponent<EnemyClass>().CatalogDesc3;
        }
    }

}

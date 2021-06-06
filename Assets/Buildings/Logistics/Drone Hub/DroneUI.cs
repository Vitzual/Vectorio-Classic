using UnityEngine;
using TMPro;

public class DroneUI : MonoBehaviour
{
    // Main UI variables
    public GameObject droneOverlay;
    public GameObject selectionUI;
    public GameObject configurationUI;
    public GameObject builderUI;
    public GameObject resourceUI;
    public GameObject combatUI;
    public GameObject fixerUI;
    public GameObject blueprintUI;
    public GameObject engineerUI;
    public Dronehub selectedPort;

    public void enableOverlay(Dronehub port)
    {
        selectedPort = port;
        droneOverlay.SetActive(true);
    }

    // Switch back to the selection UI
    public void switchToSelection()
    {
        builderUI.SetActive(false);
        resourceUI.SetActive(false);
        combatUI.SetActive(false);
        configurationUI.SetActive(false);
        selectionUI.SetActive(true);
    }

    public void enableBuilderUI()
    {
        // Set stats and object to active
        builderUI.SetActive(true);
        builderUI.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Research.research_construction_speed + "/ms";
        builderUI.transform.Find("Max Buildings").GetComponent<TextMeshProUGUI>().text = Research.research_construction_placements + " per deployment";

        // Set main UI
        selectionUI.SetActive(false);
        configurationUI.SetActive(true);
    }


    public void enableResourceUI()
    {
        // Set stats and object to active
        resourceUI.SetActive(true);
        resourceUI.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Research.research_resource_speed + "/ms";
        resourceUI.transform.Find("Max Buildings").GetComponent<TextMeshProUGUI>().text = Research.research_resource_collections + " per deployment";

        // Set main UI
        selectionUI.SetActive(false);
        configurationUI.SetActive(true);
    }

    public void enableCombatUI()
    {
        // Set stats and object to active
        combatUI.SetActive(true);
        combatUI.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Research.research_combat_speed + "/ms";
        combatUI.transform.Find("Max Targets").GetComponent<TextMeshProUGUI>().text = Research.research_combat_targets + " per deployment";

        // Set main UI
        selectionUI.SetActive(false);
        configurationUI.SetActive(true);
    }

    public void enableFixerUI()
    {
        // Set stats and object to active
        fixerUI.SetActive(true);
        fixerUI.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Research.research_combat_speed + "/ms";
        fixerUI.transform.Find("Max Buildings").GetComponent<TextMeshProUGUI>().text = Research.research_combat_targets + " per deployment";

        // Set main UI
        selectionUI.SetActive(false);
        configurationUI.SetActive(true);
    }

    public void enableBlueprintUI()
    {
        // Set stats and object to active
        blueprintUI.SetActive(true);
        blueprintUI.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Research.research_combat_speed + "/ms";
        blueprintUI.transform.Find("Max Buildings").GetComponent<TextMeshProUGUI>().text = Research.research_combat_targets + " per deployment";

        // Set main UI
        selectionUI.SetActive(false);
        configurationUI.SetActive(true);
    }

    public void enableEngineerUI()
    {
        // Set stats and object to active
        engineerUI.SetActive(true);
        engineerUI.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Research.research_combat_speed + "/ms";
        engineerUI.transform.Find("Max Buildings").GetComponent<TextMeshProUGUI>().text = Research.research_combat_targets + " per deployment";

        // Set main UI
        selectionUI.SetActive(false);
        configurationUI.SetActive(true);
    }

}

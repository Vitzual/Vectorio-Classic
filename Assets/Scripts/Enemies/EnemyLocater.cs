using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLocater : MonoBehaviour
{
    public Survival srv;
    public bool isMenu = false;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;
    }

    public Transform requestTarget(Vector3 position, Transform PreferredTarget)
    {
        /*
        Transform result = null;
        float closest = float.PositiveInfinity;
        bool isTarget = false;

        if (isMenu) return null;

        foreach (Transform collider in BuildingSystem.activeBuildings)
        {
            if (collider == null) continue;

            if (PreferredTarget != null && isTarget && collider.name != PreferredTarget.name)
                continue;

            float distance = (collider.position - position).sqrMagnitude;

            if (PreferredTarget != null && !isTarget && collider.name == PreferredTarget.name)
            {
                result = collider;
                closest = distance;
                isTarget = true;
                continue;
            }

            if (distance < closest) {
                result = collider;
                closest = distance;
            }
        }
        return result;
        */
        Debug.LogError("Hey buddy you need to remake this");
        return null;
    }
}

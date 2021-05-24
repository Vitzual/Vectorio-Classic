using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulties : MonoBehaviour
{
    // World
    public static string world;
    public static string seed;
    public static string mode;
    public static string version;

    // Resources
    public static float goldMulti;
    public static float essenceMulti;
    public static float iridiumMulti;

    // Enemies
    public static float enemyAmountMulti;
    public static float enemyHealthMulti;
    public static float enemyDamageMulti;
    public static bool enemyOutposts;
    public static bool enemyWaves;
    public static bool enemyGuardians;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            DontDestroyOnLoad(transform.gameObject);
    }
}

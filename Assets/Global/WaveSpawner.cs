using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {ACTIVE, WAITING, INACTIVE}
    public GameObject survival;
    public Canvas overlay;
    public GameObject unlockOverlay;
    public TextMeshProUGUI backup;

    [System.Serializable]
    public class Wave
    {
        public Transform[] enemies;
        public int[] amount;
        public float[] rate;
        public int minRotation;
        public int maxRotation;
        public GameObject unlock;
        public ButtonManagerBasicIcon button;
        public string unlockDescription;
    }

    public Wave[] waves;
    private Wave lastWave;
    private int currentWave = 0;
    private float checkEnemies = 1f;
    private SpawnState state = SpawnState.INACTIVE;

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!enemiesAlive())
            {
                state = SpawnState.INACTIVE;
                Debug.Log("Finished wave " + currentWave);
                overlay.transform.Find("Wave").GetComponent<CanvasGroup>().interactable = true;
                currentWave += 1;
            }
            else
            {
                return;
            }
        }

        if (state == SpawnState.ACTIVE)
        {
            StartCoroutine(SpawnWave(waves[currentWave]));
            state = SpawnState.WAITING;
        }
    }

    public bool enemiesAlive()
    {
        checkEnemies -= Time.deltaTime;
        if (checkEnemies <= 0f) {
            checkEnemies = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                unlockDefense(lastWave.unlock, lastWave.button, lastWave.unlockDescription);
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        overlay.transform.Find("Wave").GetComponent<CanvasGroup>().interactable = false;

        for (int a = 0; a < _wave.enemies.Length; a++)
        {
            for (int b = 0; b < _wave.amount[a]; b++)
            {
                SpawnEnemy(_wave.enemies[a], a, survival.GetComponent<Survival>().getDistance() + Random.Range(25f, 35f));
                yield return new WaitForSeconds(1f/_wave.rate[a]);
            }
        }

        lastWave = _wave;
        yield break;
    }

    void SpawnEnemy(Transform _enemy, int a, float b)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(waves[a].minRotation, waves[a].maxRotation)));
        transform.position += transform.right * b;
        Instantiate(_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = new Vector3(0, 0, 0);
    }

    public void setWavesActive()
    {
        state = SpawnState.ACTIVE;
    }

    public void unlockDefense(GameObject a, ButtonManagerBasicIcon b, string c)
    {
        survival.GetComponent<Survival>().addUnlocked(a);
        b.normalIcon.sprite = Resources.Load<Sprite>("Sprites/" + a.name);
        ModalWindowManager uol = unlockOverlay.GetComponent<ModalWindowManager>();
        uol.icon = Resources.Load<Sprite>("Sprites/" + a.name);
        uol.titleText = a.name;
        uol.descriptionText = c;
        backup.text = c;
        uol.OpenWindow();
    }

    public void closeOverlay()
    {
        unlockOverlay.GetComponent<ModalWindowManager>().CloseWindow();
    }
}

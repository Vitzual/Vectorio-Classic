using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    public static int MAX_SAVES = 10;

    // Menu stuff
    public GameObject SaveButtons;
    public GameObject MenuButtons;
    public GameObject GameButtons;
    public GameObject NewGame;
    public GameObject NewSaveGame;
    public GameObject LoadingSave;
    public GameObject PatreonObj;
    public CanvasGroup Title;
    public CanvasGroup Subtitle;
    public TextMeshProUGUI SaveName;
    public ModalWindowManager ConfirmDelete;
    public Settings settings;
    public bool SaveSelected = false;
    public bool NewSelected = false;
    public bool SettingsOpen = false;
    public bool PatreonOpen = false;
    public bool GameOpen = false;
    public int savesOnRecord = 0;
    public int lookingAtSave = -1;
    public Transform saveList;

    // Difficulty variables
    public TMP_InputField WorldName;
    public TMP_InputField WorldSeed;
    public SliderManager GoldMulti;
    public SliderManager EssenceMulti;
    public SliderManager IridiumMulti;
    public SliderManager EnemiesAmount;
    public SliderManager EnemiesHealth;
    public SliderManager EnemiesDamage;
    public Toggle EnemyOutposts;
    public Toggle EnemyGroups;
    public Toggle EnemyGuardians;

    [System.Serializable]
    public class PresetDifficulties
    {
        public string presetName;
        public float goldMulti;
        public float essenceMulti;
        public float iridiumMulti;
        public float enemyAmountMulti;
        public float enemyHealthMulti;
        public float enemyDamageMulti;
        public float enemySpeedMulti;
        public bool enemyOutposts;
        public bool enemyWaves;
        public bool enemyGuardians;
    }
    public List<PresetDifficulties> difficulties;

    public void Start()
    {
        WorldName.characterLimit = 12;
        WorldSeed.characterLimit = 10;

        Time.timeScale = 1f;
        Application.targetFrameRate = 300;

        settings.LoadSettings();
        UpdateSaves();
    }

    public void RandomSeed()
    {
        WorldSeed.text = Random.Range(10000, 99999).ToString() + Random.Range(10000, 99999).ToString();
    }

    public void UpdateSaves()
    {
        Transform holder;
        bool availableSave = false;

        for (int i = 1; i <= MAX_SAVES; i++)
        {
            if (SaveSystem.CheckForSave(i))
            {
                holder = saveList.Find("Save " + i).GetComponent<Transform>();

                string[] strs = SaveSystem.GetSaveStrings(i);
                holder.GetComponent<ButtonManagerBasic>().buttonText = strs[0];
                holder.Find("Difficulty").GetComponent<TextMeshProUGUI>().text = strs[1];
                holder.Find("Heat").GetComponent<TextMeshProUGUI>().text = strs[2];

                // Calculate and set timer
                try
                {
                    int time = int.Parse(strs[3]);
                    int hours = System.TimeSpan.FromSeconds(time).Hours;
                    int minutes = System.TimeSpan.FromSeconds(time).Minutes;
                    int seconds = System.TimeSpan.FromSeconds(time).Seconds;

                    string mstring;
                    string sstring;
                    if (minutes <= 9) mstring = "0" + minutes;
                    else mstring = minutes.ToString();
                    if (seconds <= 9) sstring = "0" + seconds;
                    else sstring = seconds.ToString();
 
                    holder.Find("Timer").GetComponent<TextMeshProUGUI>().text = hours + ":" + mstring + ":" + sstring;
                }
                catch
                {
                    holder.Find("Timer").GetComponent<TextMeshProUGUI>().text = "0:00:00";
                }

                holder.GetComponent<ButtonManagerBasic>().UpdateUI();
                holder.gameObject.SetActive(true);
            }
            else
            {
                saveList.Find("Save " + i).GetComponent<Transform>().gameObject.SetActive(false);
                availableSave = true;
            }
        }

        if (!availableSave) NewSaveGame.SetActive(false);
        else NewSaveGame.SetActive(true);
    }

    public void OpenDeleteSaveMenu(int a)
    {
        lookingAtSave = a;
        ConfirmDelete.OpenWindow();
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteGame(lookingAtSave);
        ConfirmDelete.CloseWindow();
        UpdateSaves();
    }

    public void CancelDeleteSave()
    {
        lookingAtSave = -1;
        ConfirmDelete.CloseWindow();
    }

    public void PlayAudio(Transform button)
    {
        AudioSource sfx = button.GetComponent<AudioSource>();
        float volume = settings.GetSound();
        if (volume == 0) sfx.volume = 0;
        else
        {
            volume -= 0.4f;
            if (volume < 0.2f) sfx.volume = 0.2f;
            else sfx.volume = volume;
        }
        sfx.Play();
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) && PatreonOpen)) DisablePatreon();
        else if ((Input.GetKeyDown(KeyCode.Escape) && SaveSelected))
        {
            GameButtons.GetComponent<CanvasGroup>().alpha = 1f;
            GameButtons.GetComponent<CanvasGroup>().interactable = true;
            GameButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

            SaveButtons.GetComponent<CanvasGroup>().alpha = 0;
            SaveButtons.GetComponent<CanvasGroup>().interactable = false;
            SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;

            SaveSelected = false;
            GameOpen = true;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && GameOpen))
        {
            MenuButtons.GetComponent<CanvasGroup>().alpha = 1f;
            MenuButtons.GetComponent<CanvasGroup>().interactable = true;
            MenuButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

            GameButtons.GetComponent<CanvasGroup>().alpha = 0;
            GameButtons.GetComponent<CanvasGroup>().interactable = false;
            GameButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameOpen = false;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && NewSelected))
        {
            SaveButtons.GetComponent<CanvasGroup>().alpha = 1f;
            SaveButtons.GetComponent<CanvasGroup>().interactable = true;
            SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

            SaveSelected = true;

            NewGame.GetComponent<CanvasGroup>().alpha = 0;
            NewGame.GetComponent<CanvasGroup>().interactable = false;
            NewGame.GetComponent<CanvasGroup>().blocksRaycasts = false;

            NewSelected = false;

            Title.alpha = 1f;
            Subtitle.alpha = 1f;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && SettingsOpen))
        {
            settings.DisableMenu();
            SettingsOpen = false;
        }
    }

    public void EnablePatreon()
    {
        PatreonOpen = true;
        SaveButtons.SetActive(false);
        MenuButtons.SetActive(false);
        GameButtons.SetActive(false);
        NewGame.SetActive(false);
        NewSaveGame.SetActive(false);
        PatreonObj.SetActive(true);
        Title.alpha = 0f;
        Subtitle.alpha = 0f;
    }

    public void DisablePatreon()
    {
        PatreonOpen = false;
        SaveButtons.SetActive(true);
        MenuButtons.SetActive(true);
        GameButtons.SetActive(true);
        NewGame.SetActive(true);
        NewSaveGame.SetActive(true);
        PatreonObj.SetActive(false);

        if (!NewSelected)
        {
            Title.alpha = 1f;
            Subtitle.alpha = 1f;
        }
    }

    public void OpenDiscordURL()
    {
        Application.OpenURL("https://discord.com/invite/auDgRJqtT9");
    }

    public void OpenRedditURL()
    {
        Application.OpenURL("https://www.reddit.com/r/Vectorio/");
    }

    public void OpenPatreonURL()
    {
        Application.OpenURL("https://www.patreon.com/vectorio");
    }

    public void OpenSoundcloudURL()
    {
        Application.OpenURL("https://soundcloud.com/airglowsounds");
    }

    public void SetSettingsState(bool a)
    {
        SettingsOpen = a;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayButton()
    {
        GameButtons.GetComponent<CanvasGroup>().alpha = 0;
        GameButtons.GetComponent<CanvasGroup>().interactable = false;
        GameButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;

        SaveButtons.GetComponent<CanvasGroup>().alpha = 1f;
        SaveButtons.GetComponent<CanvasGroup>().interactable = true;
        SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameOpen = false;
        SaveSelected = true;
        UpdateSaves();
    }

    public void GameButton()
    {
        MenuButtons.GetComponent<CanvasGroup>().alpha = 0;
        MenuButtons.GetComponent<CanvasGroup>().interactable = false;
        MenuButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;

        GameButtons.GetComponent<CanvasGroup>().alpha = 1f;
        GameButtons.GetComponent<CanvasGroup>().interactable = true;
        GameButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

        SaveSelected = false;
        GameOpen = true;
    }

    public void SelectSaveNumber(int i)
    {
        lookingAtSave = i;
        SelectSave();
    }

    public void NewSave()
    {
        for (int i = 1; i <= MAX_SAVES; i++)
            if (!SaveSystem.CheckForSave(i))
            {
                lookingAtSave = i;
                SelectSave();
                return;
            }
        Debug.Log("No available save slots");
    }

    public void SelectSave()
    {
        string SavePath = Application.persistentDataPath + "/world_" + lookingAtSave + ".save";

        SetGameSave(lookingAtSave);

        if (File.Exists(SavePath))
        {
            LoadingSave.SetActive(true);
            SceneManager.LoadScene("Survival");
        }
        else
        {
            NewGame.GetComponent<CanvasGroup>().alpha = 1f;
            NewGame.GetComponent<CanvasGroup>().interactable = true;
            NewGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            NewSelected = true;

            SaveButtons.GetComponent<CanvasGroup>().alpha = 0f;
            SaveButtons.GetComponent<CanvasGroup>().interactable = false;
            SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;
            SaveSelected = false;

            Title.alpha = 0f;
            Subtitle.alpha = 0f;
        }

    }

    public void SetDifficulty(int ID)
    {
        GoldMulti.mainSlider.value = difficulties[ID].goldMulti;
        EssenceMulti.mainSlider.value = difficulties[ID].essenceMulti;
        IridiumMulti.mainSlider.value = difficulties[ID].iridiumMulti;
        EnemiesAmount.mainSlider.value = difficulties[ID].enemyAmountMulti;
        EnemiesHealth.mainSlider.value = difficulties[ID].enemyHealthMulti;
        EnemiesDamage.mainSlider.value = difficulties[ID].enemyDamageMulti;
        EnemyOutposts.isOn = difficulties[ID].enemyOutposts;
        EnemyGroups.isOn = difficulties[ID].enemyWaves;
        EnemyGuardians.isOn = difficulties[ID].enemyGuardians;
    }

    public void StartNewGame()
    {
        Difficulties.world = WorldName.text;
        Difficulties.seed = WorldSeed.text;
        Difficulties.mode = "Version 0.2";
        Difficulties.goldMulti = GoldMulti.mainSlider.value;
        Difficulties.essenceMulti = EssenceMulti.mainSlider.value;
        Difficulties.iridiumMulti = IridiumMulti.mainSlider.value;
        Difficulties.enemyAmountMulti = EnemiesAmount.mainSlider.value;
        Difficulties.enemyHealthMulti = EnemiesHealth.mainSlider.value;
        Difficulties.enemyDamageMulti = EnemiesDamage.mainSlider.value;
        Difficulties.enemySpeedMulti = 1f;
        Difficulties.enemyOutposts = EnemyOutposts.isOn;
        Difficulties.enemyWaves = EnemyGroups.isOn;
        Difficulties.enemyGuardians = EnemyGuardians.isOn;

        LoadingSave.SetActive(true);
        SceneManager.LoadScene("Survival");
    }

    public void SetGameSave(int a)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/location.vectorio";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, "/world_" + a+".save");
        stream.Close();
    }
}

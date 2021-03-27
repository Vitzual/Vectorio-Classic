using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Menu : MonoBehaviour
{
    public static int MAX_SAVES = 10;

    public GameObject MainMenu;
    public GameObject SaveButtons;
    public GameObject MenuButtons;
    public GameObject GameButtons;
    public GameObject NewGame;
    public GameObject NewSaveGame;
    public GameObject NameNewSave;
    public TextMeshProUGUI SaveName;
    public TMP_InputField SaveBox;
    public ModalWindowManager ConfirmDelete;
    public Settings settings;
    public bool SaveSelected = false;
    public bool NewSelected = false;
    public bool SettingsOpen = false;
    public bool GameOpen = false;
    public int savesOnRecord = 0;
    public int lookingAtSave = -1;

    public Transform saveList;

    public void Start()
    {
        SaveBox.characterLimit = 13;

        Time.timeScale = 1f;
        Application.targetFrameRate = 300;

        settings.LoadSettings();
        UpdateSaves();
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
                    holder.Find("Timer").GetComponent<TextMeshProUGUI>().text = hours + ":" + minutes + ":" + seconds;
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
        Debug.Log(button.name);
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
        if (Input.GetKeyDown(KeyCode.Escape) && NameNewSave.activeInHierarchy)
        {
            NameNewSave.SetActive(false);
        }
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

            transform.Find("Title").GetComponent<CanvasGroup>().alpha = 1f;
            transform.Find("Subtitle").GetComponent<CanvasGroup>().alpha = 1f;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && SettingsOpen))
        {
            settings.DisableMenu();
            SettingsOpen = false;
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

    public void SetName()
    {
        GameObject.Find("Difficulty").GetComponent<Difficulties>().SetSaveName(SaveName.text);
        NameNewSave.SetActive(false);
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
                NameNewSave.SetActive(true);
                return;
            }
        Debug.Log("No available save slots");
    }

    public void SelectSave()
    {
        string SavePath = Application.persistentDataPath + "/save_" + lookingAtSave + ".vectorio";

        if (File.Exists(SavePath))
        {
            SetGameSave(lookingAtSave);
            SceneManager.LoadScene("Survival");
        }
        else
        {
            SetGameSave(lookingAtSave);

            NewGame.GetComponent<CanvasGroup>().alpha = 1f;
            NewGame.GetComponent<CanvasGroup>().interactable = true;
            NewGame.GetComponent<CanvasGroup>().blocksRaycasts = true;
            NewSelected = true;

            SaveButtons.GetComponent<CanvasGroup>().alpha = 0f;
            SaveButtons.GetComponent<CanvasGroup>().interactable = false;
            SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;
            SaveSelected = false;

            transform.Find("Title").GetComponent<CanvasGroup>().alpha = 0f;
            transform.Find("Subtitle").GetComponent<CanvasGroup>().alpha = 0f;
        }

    }

    public void StartNewGame(int a)
    {
        Difficulties dd = GameObject.Find("Difficulty").GetComponent<Difficulties>();

        if (a == 0) dd.SetModeName("CUSTOM");
        else if (a == 1) dd.SetModeName("EASY");
        else if (a == 2) dd.SetModeName("NORMAL");
        else if (a == 3) dd.SetModeName("HARD");
        else if (a == 4) dd.SetModeName("NIGHTMARE");

        dd.SetDifficulty(a);
        SceneManager.LoadScene("Survival");
    }

    public void SetGameSave(int a)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/location.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, "/save_"+a+".vectorio");
        stream.Close();
    }
}

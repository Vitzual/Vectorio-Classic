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
    public GameObject NewGame;
    public GameObject NewSaveGame;
    public ModalWindowManager ConfirmDelete;
    public Settings settings;
    public bool SaveSelected = false;
    public bool NewSelected = false;
    public bool SettingsOpen = false;
    public int savesOnRecord = 0;
    public int lookingAtSave = -1;

    public Transform saveList;

    public void Start()
    {
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
                holder.Find("Timer").GetComponent<TextMeshProUGUI>().text = strs[3];
                holder.GetComponent<ButtonManagerBasic>().UpdateUI();
                holder.gameObject.SetActive(true);
            }
            else availableSave = true;
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
        if ((Input.GetKeyDown(KeyCode.Escape) && SaveSelected))
        {
            MenuButtons.GetComponent<CanvasGroup>().alpha = 1f;
            MenuButtons.GetComponent<CanvasGroup>().interactable = true;
            MenuButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

            SaveButtons.GetComponent<CanvasGroup>().alpha = 0;
            SaveButtons.GetComponent<CanvasGroup>().interactable = false;
            SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;

            SaveSelected = false;
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
        MenuButtons.GetComponent<CanvasGroup>().alpha = 0;
        MenuButtons.GetComponent<CanvasGroup>().interactable = false;
        MenuButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;

        SaveButtons.GetComponent<CanvasGroup>().alpha = 1f;
        SaveButtons.GetComponent<CanvasGroup>().interactable = true;
        SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

        SaveSelected = true;
        UpdateSaves();
    }

    public void NewSave()
    {
        for (int i = 1; i <= MAX_SAVES; i++)
            if (!SaveSystem.CheckForSave(i))
            {
                SelectSave(i);
                return;
            }
        Debug.Log("No available save slots");
    }


    public void SelectSave(int a)
    {
        string SavePath = Application.persistentDataPath + "/save_" + a + ".vectorio";

        if (File.Exists(SavePath))
        {
            SetGameSave(a);
            SceneManager.LoadScene("Survival");
        }
        else
        {
            SetGameSave(a);

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
        GameObject.Find("Difficulty").GetComponent<Difficulties>().SetDifficulty(a);
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

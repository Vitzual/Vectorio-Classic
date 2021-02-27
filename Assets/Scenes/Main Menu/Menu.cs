using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Michsky.UI.ModernUIPack;

public class Menu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject SaveButtons;
    public GameObject MenuButtons;
    public GameObject NewGame;
    public bool SaveSelected = false;
    public bool NewSelected = false;

    public void Start()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 300;
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

        // See what saves are on record
        for (int i=1; i<6; i++)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string SavePath = Application.persistentDataPath + "/save_" + i + ".vectorio";

            if (File.Exists(SavePath))
            {
                SaveButtons.transform.Find("Save " + i).GetComponent<ButtonManager>().buttonText = "Save " + i;
                SaveButtons.transform.Find("Save " + i).GetComponent<ButtonManager>().UpdateUI();
            }
        }
    }

    public void ResetButton(int a)
    {
        string SavePath = Application.persistentDataPath + "/save_" + a + ".vectorio";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);

        }
    }

    public void SelectSave(int a)
    {
        BinaryFormatter formatter = new BinaryFormatter();
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

    public void SetGameSave(int a)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/location.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, "/save_"+a+".vectorio");
        stream.Close();
    }
}

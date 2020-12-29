using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Menu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject SaveButtons;
    public GameObject MenuButtons;

    public void Start()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 300;
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
    }

    public void ResetButton(int a)
    {
        string SavePath = Application.persistentDataPath + "/save" + a + ".vectorio";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);

        }
    }

    public void BackButton()
    {
        MenuButtons.GetComponent<CanvasGroup>().alpha = 1f;
        MenuButtons.GetComponent<CanvasGroup>().interactable = true;
        MenuButtons.GetComponent<CanvasGroup>().blocksRaycasts = true;

        SaveButtons.GetComponent<CanvasGroup>().alpha = 0;
        SaveButtons.GetComponent<CanvasGroup>().interactable = false;
        SaveButtons.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void StartGame(int a)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/location.save";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, "/save"+a+".vectorio");
        stream.Close();

        SceneManager.LoadScene("Survival");
    }
}

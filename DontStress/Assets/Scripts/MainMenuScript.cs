using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// public class MainMenuScript : MonoBehaviour
// {
//     public void PlayGame()
//     {
//         SceneManager.LoadSceneAsync("Game");
//     }
// }

public class MainMenuScript : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject starterBook;

    public GameObject mainCamera;
    public GameObject eventSystems; 
    private bool gameSceneLoaded = false;
    private bool oldGameAv = true;

    private void Awake()
    {
        if (!File.Exists(GlobalVariables.saveDataPath))
        {
            oldGameAv = false;
            continueButton.SetActive(false);
        }

        ManageEventSystems();
        ManageAudioListeners();
    }

    public void ContinueGame()
    {
        // eventSystems.SetActive(false);
        // mainCamera.GetComponent<AudioListener>().enabled = false;
        // mainCamera.SetActive(false);
        // SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        string path = GlobalVariables.saveDataPath;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveDataClass data = formatter.Deserialize(stream) as SaveDataClass;
            stream.Close();
            SceneManager.LoadSceneAsync("StarterVillage");
            gameSceneLoaded = true;
        }
    }

    public void NewGame()
    {
        List<GameObject> starterBooks = new List<GameObject>();
        starterBooks.Add(starterBook);
        SaveSystem.SaveGame(starterBooks);
        SceneManager.LoadSceneAsync("StarterVillage");
        gameSceneLoaded = true;
    }

    private void ResumeGame()
    {
        GlobalVariables.Paused = false;
        Time.timeScale = 1f;
    }
    private void PauseGame()
    {
        GlobalVariables.Paused = false;
        Time.timeScale = 0f;
    }

     public void ReturnToMenu()
    {
        if (gameSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("Game");
            gameSceneLoaded = false;
        }
        eventSystems.SetActive(true);
        mainCamera.GetComponent<AudioListener>().enabled = true;
        mainCamera.SetActive(true);
    }

    #region Management
    private void ManageEventSystems()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

        // If more than one EventSystem is found, disable all but one
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                eventSystems[i].gameObject.SetActive(false);
                Debug.LogWarning("Disabled extra EventSystem on: " + eventSystems[i].gameObject.name);
            }
        }
    }

    private void ManageAudioListeners()
    {
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // If more than one AudioListener is found, disable all but one
        if (audioListeners.Length > 1)
        {
            for (int i = 1; i < audioListeners.Length; i++)
            {
                audioListeners[i].gameObject.SetActive(false);
                Debug.LogWarning("Disabled extra AudioListener on: " + audioListeners[i].gameObject.name);
            }
        }
    }
    #endregion
    public void QuitGame()
    {
        Application.Quit();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    static bool isPaused;

    public AudioSource CarStart;
    public AudioSource CarStop;
    public AudioSource CarEngine;
    public AudioSource BackGroundMusic;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    IEnumerator playEngineSound()
    {
        yield return new WaitForSeconds(.7f);
        CarEngine.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Resume();
                
            }
            else
            {
                Pause();
               
            }
        }


    }

    void Pause()
    {
        pausePanel.SetActive(true);
        CarEngine.Pause();
        CarStop.Play();
        BackGroundMusic.Pause(); // Add this line to pause the background music
        Time.timeScale = 0f;
        isPaused = true;
    }

    void Resume()
    {
        pausePanel.SetActive(false);
        CarStart.Play();
        StartCoroutine(playEngineSound());
        BackGroundMusic.Play(); // Use Play() to resume the background music
        Time.timeScale = 1f;
        isPaused = false;
    }
    
}
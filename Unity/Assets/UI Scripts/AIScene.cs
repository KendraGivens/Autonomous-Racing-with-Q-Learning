using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AIScene : MonoBehaviour
{
    public Button NeuralNetsButton;

    void Start()
    {
        NeuralNetsButton.onClick.AddListener(StartAIGame);
    }

    void StartAIGame()
    {
        SceneManager.LoadScene("AI_Scene");
        Agent.AI_Controlled = true;
    }

}

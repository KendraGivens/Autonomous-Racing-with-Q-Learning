using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NeuralNetsButtonController : MonoBehaviour
{
    public Button NeuralNetsButton;

    void Start()
    {
        NeuralNetsButton.onClick.AddListener(StartAIGame);
    }

    void StartAIGame()
    {
        SceneManager.LoadScene("CoastalTrackDisplayScene"); // Replace "RaceTrackScene" with the name of your race track scene.
       // AI_Controlled.Enabled = false;
    }
}

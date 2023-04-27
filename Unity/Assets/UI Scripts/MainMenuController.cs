using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void OnMenuButtonClick()
    {
        // Resume the game by setting timeScale to 1
        Time.timeScale = 1;
        SceneManager.LoadScene("RacingUI");
    }
}

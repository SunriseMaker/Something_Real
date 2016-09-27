using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel(string level_tag)
    {
        //int current_level = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(level_tag);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

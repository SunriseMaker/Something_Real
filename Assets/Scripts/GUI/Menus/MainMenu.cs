using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel(string level_tag)
    {
        __General.LoadLevel(level_tag);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

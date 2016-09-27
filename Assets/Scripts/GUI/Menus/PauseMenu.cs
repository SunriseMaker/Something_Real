using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    const int MAIN_MENU = 0;

    #region MonoBehaviour
    private void Awake()
    {
        __Time.PauseGame();

        Transform selected_button = __General.FindChildRecursively(transform, "Button_ResumeGame");

        GameData.Singletons.event_system.SetSelectedGameObject(selected_button.gameObject);
    }
    #endregion MonoBehaviour

    #region Red
    public void ResumeGame()
    {
        __Time.UnpauseGame();
        Destroy(gameObject);
    }

    public void RestartLevel()
    {
        __Time.UnpauseGame();
        int current_level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current_level);
        Destroy(gameObject);
    }

    public void QuitToMainMenu()
    {
        __Time.UnpauseGame();
        SceneManager.LoadScene(MAIN_MENU);
        Destroy(gameObject);
    }
    #endregion Red
}

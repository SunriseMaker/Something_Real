using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region MonoBehaviour
    private void Awake()
    {
        __Time.PauseGame();
        GameData.Singletons.main_camera.BlurEffect(true);

        Button first_button = __General.FindChildRecursively(transform, "Button_ResumeGame").GetComponent<Button>();
        first_button.Select();
        first_button.OnSelect(new UnityEngine.EventSystems.BaseEventData(GameData.Singletons.event_system));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ResumeGame();
        }
    }
    #endregion MonoBehaviour

    #region Red
    public void ResumeGame()
    {
        StartCoroutine(crResumeGame());
    }

    private System.Collections.IEnumerator crResumeGame()
    {
        yield return new WaitForEndOfFrame();

        GameData.Singletons.main_camera.BlurEffect(false);
        __Time.UnpauseGame();
        Destroy(gameObject);
    }

    public void RestartLevel()
    {
        int current_level_index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(crUnpauseAndLoad(current_level_index));
    }

    public void QuitToMainMenu()
    {
        StartCoroutine(crUnpauseAndLoad(GameData.main_menu_id));
    }

    private System.Collections.IEnumerator crUnpauseAndLoad(int level_index)
    {
        yield return new WaitForEndOfFrame();
        __Time.UnpauseGame();
        __General.LoadLevel(level_index);
    }
    #endregion Red
}

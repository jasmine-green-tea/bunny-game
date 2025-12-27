using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {

    }

    public void OnPlayClicked()
    {
        LoadGameScene();
    }

    public void OnLoadClicked()
    {
        // For now, just load the game scene like Play
        // Later you can add loading logic here
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitClicked()
    {
        Application.Quit();

        // For testing in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
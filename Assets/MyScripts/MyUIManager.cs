using UnityEngine;
using UnityEngine.SceneManagement;

public class MyUIManager : MonoBehaviour

{
    public GameObject pauseUI;
    public void OnRestartPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


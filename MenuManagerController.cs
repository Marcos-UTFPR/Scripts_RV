using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerController : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void returnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

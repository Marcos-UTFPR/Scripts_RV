using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

// OBS: Teclas 1 e 2 muda a posição da câmera, tecla espaço controla o minimap, mouse controla os jogadores
public class GameManagerController : MonoBehaviour
{
    public Text txtGreen; // Text contendo o placar do time verde
    public int scoreGreen = 0;
    public Text txtOrange; // Text contendo o placar do time laranja
    public int scoreOrange = 0;
    public Text txtMiddleScore; // X no meio do placar
    public string currentRound = "Green";
    public GameObject minimap;
    public Image minimapColor;
    private bool isMinimapActive;
    private bool isPaused; // Flag usada para saber se está pausado ou não
    private bool isBlocked; // Flag usada para prevenir um bug que permite pausar na derrota/vitória e descongelar o jogo
    public GameObject pauseCanvas;
    public GameObject endCanvas;
    public bool editorMode = false; // Flag que indica se é editor ou não
    public Text txtJogadas;
    public int maxJogadas; // Número máximo de jogadas antes de trocar de jogador (Default: 10)
    public int jogadas;
    public Camera mainCamera;
    public AudioSource goalSound;
    private Transform mainCameraTransform;
    public static event Action resetTeamPosition; // Evento para avisar os jogadores para resetar a posição deles após um gol
    public int maxGoals; // Número máximo de gols para vencer o jogo
    public GameObject goalSprite;
    

    void Start()
    {
        txtGreen.text = scoreGreen.ToString();
        txtOrange.text = scoreOrange.ToString();
        isMinimapActive = true;
        minimap.SetActive(true);
        isPaused = false;
        isBlocked = false;
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        goalSprite.SetActive(false);
        jogadas = 0;
        updateScoreColor();
        mainCameraTransform = mainCamera.GetComponent<Transform>();
        if (Application.isEditor) // Dá para controlar ambos os times no editor
        {
            editorMode = true;
            Debug.Log("We're running this from inside of the editor!");
        } else {
            editorMode = false;
        }
    }

    void updateScoreColor()
    {
        Color newColor;
        if (currentRound == "Green")
        {
            newColor = new Color(0f, 0.788f, 0f, 1f); // Green
        } else {
            newColor = new Color(0.886f, 0.251f, 0f, 1f); // Orange
        }
        //txtMiddleScore.color = newColor;
        minimapColor.color = newColor;
        txtJogadas.color = newColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBlocked)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                triggerMinimap(); // Ativar ou desativar o minimap
            }
            // de 1 a até 4 para mudar as cameras
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "FirstCamera");
                mainCameraTransform.position = new Vector3(0f, 20f, 28f);
                mainCameraTransform.rotation = Quaternion.Euler(40f, 180f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "SecondCamera");
                mainCameraTransform.position = new Vector3(0f, 20f, -28f);
                mainCameraTransform.rotation = Quaternion.Euler(40f, 0f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
                mainCameraTransform.position = new Vector3(-0.28f, 26f, 0f);
                mainCameraTransform.rotation = Quaternion.Euler(90f, 180f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                mainCamera.cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
                mainCameraTransform.position = new Vector3(-0.28f, 26f, 0f);
                mainCameraTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
            }
        }

    }

    void OnEnable()
    {
        BallController.greenGoal += greenGoal;
        BallController.orangeGoal += orangeGoal;
        BallController.changePlayerTurn += changeTurn;
        PlayerController.addJogada += aumentaJogadas;
    }

    void OnDisable()
    {
        BallController.greenGoal -= greenGoal;
        BallController.orangeGoal -= orangeGoal;
        BallController.changePlayerTurn -= changeTurn;
        PlayerController.addJogada -= aumentaJogadas;
    }

    private void triggerMinimap()
    {
        if (!isMinimapActive)
        {
            minimap.SetActive(true);
            isMinimapActive = true;
        } else {
            minimap.SetActive(false);
            isMinimapActive = false;
        }
    }

    public void Disable(GameObject obj, float time) // Admito que peguei esse do GPT
    {
        StartCoroutine(DisableRoutine(obj, time));
    }

    IEnumerator DisableRoutine(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    void greenGoal()
    {
        scoreGreen += 1;
        txtGreen.text = scoreGreen.ToString();
        postGoalEffects();
    }

    void orangeGoal()
    {
        scoreOrange += 1;
        txtOrange.text = scoreOrange.ToString();
        postGoalEffects();
    }

    void postGoalEffects()
    {
        if (!exibeFimJogo()){ // Só faz as modificações de gol caso não tive acabado o jogo ainda
            Debug.Log("Gol (Sem Vitória)");
            goalSprite.SetActive(true);
            goalSound.Play();
            changeTurn();
            resetTeamPosition?.Invoke();
            Disable(goalSprite, 4f);
        }
    }

    void aumentaJogadas()
    {
        jogadas += 1;
        onJogadasUpdate();
        if (jogadas >= maxJogadas)
            changeTurn(); // Se chegar no limite de jogadas, troca o turno
    }

    void changeTurn()
    {
        if (currentRound == "Green")
        {
            currentRound = "Orange";
        } else {
            currentRound = "Green";
        }
        updateScoreColor();
        jogadas = 0;
        onJogadasUpdate();
    }
    void onJogadasUpdate()
    {
        txtJogadas.text = jogadas + " / " + maxJogadas;
    }
    private bool exibeFimJogo() // OBS: Reiniciar jogo devia ficar em um script GameManagement, não em PlayerController
    {
        if (scoreGreen >= maxGoals || scoreOrange >= maxGoals)
        {
            isBlocked = true; // Impede que pause o jogo na tela de vitória e descongele o jogo após despausar
            Time.timeScale = 0;
            Debug.Log("Fim de jogo!");
            if (scoreGreen > scoreOrange)
            {
                Debug.Log("Vitória Verde!");
            } else {
                Debug.Log("Vitória Laranja!");
            }
            endCanvas.SetActive(true);
            mainCamera.GetComponent<AudioSource>().Play();
            return true;
        } else {
            return false;
        }
    }

    private void pauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseCanvas.SetActive(true);
        } else {
            resumeGame();
        }
    }

    public void resumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseCanvas.SetActive(false);
        }
    }

    public void ReiniciarFase() // Tem que ser público para acessar pelo botão
    {
        RestartScene();
    }

    private void RestartScene()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void returnMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}

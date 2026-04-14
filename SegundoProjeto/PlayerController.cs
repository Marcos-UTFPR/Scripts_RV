using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Text txtScore; // OBS: Text é um GameObject - Text contendo a pontuação
    public Text txtDefeat; // Text contendo a tela de derrota
    public Text txtVictory; // Text contendo a tela de vitória
    public Text txtPause; // Text contendo a tela de pausa
    private bool isPaused; // Flag usada para saber se está pausado ou não
    private bool isBlocked; // Flag usada para prevenir um bug que permite pausar na derrota/vitória e descongelar o jogo
    public Button buttonRestart;
    public Button buttonTryAgain;
    private int score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Tempo do jogo: "+Time.timeScale.ToString());
        Time.timeScale = 1;
        txtScore.text = "Placar: " + score.ToString();
        score = 0;
        isPaused = false;
        isBlocked = false;
        txtVictory.gameObject.SetActive(false);
        buttonRestart.gameObject.SetActive(false);
        txtDefeat.gameObject.SetActive(false);
        buttonTryAgain.gameObject.SetActive(false);
        txtPause.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        BulletController.hitEnemy += addPoint;
        CubeTrapController.hitPlayer += defeat;
        EvilBulletController.hitPlayer += defeat;
    }

    void OnDisable()
    {
        BulletController.hitEnemy -= addPoint;
        CubeTrapController.hitPlayer -= defeat;
        EvilBulletController.hitPlayer -= defeat;
    }

    // Update is called once per frame
    void Update() // Movimento do jogador
    {
        // deltaTime � o tempo entre os frames
        if (!isBlocked)
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f; // x vai ser a rota��o do jogador
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame();
            }
        }
        
    }

    private void Fire()
    {
        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawn.position,
                                 bulletSpawn.rotation);
        AudioSource shoot = GetComponent<AudioSource>(); // OBS: Pega o primeiro AudioSource que tem no GameObject desse script
        shoot.Play();
        bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * 6.0f;
        Destroy(bullet, 3f); // (GameObject, tempo em segundos) - OBS: Funciona em Thread separada
        
    }
    public void addPoint()
    {
        score++;
        Debug.Log("Placar: " + score.ToString());
        txtScore.text = "Placar: " + score.ToString(); // Mudando o texto que est� nesse Text
        exibeFimJogo(); // Testa se chegou no fim de jogo
    }

    private void exibeFimJogo() // OBS: Reiniciar jogo devia ficar em um script GameManagement, não em PlayerController
    {
        if (score >= 3)
        {
            isBlocked = true; // Impede que pause o jogo na tela de vitória e descongele o jogo após despausar
            Time.timeScale = 0;
            Debug.Log("Fim de jogo!");
            txtVictory.gameObject.SetActive(true);
            buttonRestart.gameObject.SetActive(true);
        }
    }

    private void defeat()
    {
        isBlocked = true; // Impede que pause o jogo na tela de derrota e descongele o jogo após despausar
        Time.timeScale = 0;
        txtDefeat.gameObject.SetActive(true);
        AudioSource death = txtDefeat.GetComponent<AudioSource>();
        death.Play();
        buttonTryAgain.gameObject.SetActive(true);
    }

    public void ReiniciarFase() // Tem que ser público para acessar pelo botão
    {
        RestartScene();
    }

    private void RestartScene()
    {
        Scene cena = SceneManager.GetActiveScene();
        SceneManager.LoadScene(cena.name);
    }

    private void pauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            txtPause.gameObject.SetActive(true);
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
            txtPause.gameObject.SetActive(false); // Linha do problema
        }
    }
}

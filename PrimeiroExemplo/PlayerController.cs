using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    private Rigidbody rb; // Componente Rigidbody para tratar ele

    public int velocidade; // Velocidade do componente

    private int placar; // Pontuação
    public Text txtPontuacao;

    public Text txtVitoria; // OBS: Text é um GameObject

    public Button txtReiniciar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Não precisa procurar a esfera ao qual esse Script está vinculado, pois já tem a referência automática
        placar = 0;
        txtVitoria.gameObject.SetActive(false);
        txtReiniciar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // LateUpdate is called once per frame, but only once it has already finished rendering
    void LateUpdate()
    {

    }

    // FixedUpdate is called once a determined period of time, useful for physics calculations
    void FixedUpdate()
    {
        // Input do usuário
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        float movimentoVertical = Input.GetAxis("Vertical");
        bool flag = Input.GetKey("space");
        float movimentoY = 0;
        if (flag) {
            movimentoY = 4; // Era pra ser pulo mas virou a habilidade de voar livremente
        }

        // Adicionar força no rigidbody baseado nesse input
        Vector3 force = new Vector3(movimentoHorizontal,movimentoY, movimentoVertical);

        rb.AddForce(force*velocidade, ForceMode.Force);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
            placar++;
            Debug.Log("Placar: " + placar.ToString());
            txtPontuacao.text = "Placar: " + placar.ToString(); // Mudando o texto que está nesse Text
            exibeFimJogo();
        }
    }

    private void exibeFimJogo() // OBS: Reiniciar jogo devia ficar em um script GameManagement, não em PlayerController
    {
        if (placar == 7)
        {
            Debug.Log("Fim de jogo!");
            txtVitoria.gameObject.SetActive(true);
            txtReiniciar.gameObject.SetActive(true);
        }
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
}

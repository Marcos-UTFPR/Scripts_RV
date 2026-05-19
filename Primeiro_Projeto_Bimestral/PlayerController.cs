using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 startPosition;
    private Rigidbody rb;
    private Vector3 limitedOffset;
    private bool isDragging = false;
    public string color;
    public float forceMultiplier = 5f; // Ajuste a força aqui
    public float maxDragDistance = 3.0f;// Distância máxima que o objeto pode ser puxado
    public GameObject gameManager;
    private GameManagerController gameManagerController;
    public static event Action addJogada;

    [Header("Arrow Settings")]
    public GameObject arrowPrefab;      // Arraste um prefab de seta aqui
    private GameObject arrowInstance;
    public Gradient arrowGradient;      // Gradiente de cor (verde → vermelho)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        gameManagerController = gameManager.GetComponent<GameManagerController>();

        // Instancia a seta já desativada
        if (arrowPrefab != null)
        {
            arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrowInstance.SetActive(false);
        }
    }

    void OnEnable() { GameManagerController.resetTeamPosition += resetPosition; }
    void OnDisable() { GameManagerController.resetTeamPosition -= resetPosition; }

    void OnMouseDown()
    {
        // Se for EditorMode, ignora o turno e permite mover jogadores de qualquer time
        if (gameManagerController.editorMode || gameManagerController.currentRound == color)
        {
            isDragging = true;
            startPosition = transform.position;

            if (arrowInstance != null)
                arrowInstance.SetActive(true);
        }
    }

    void OnMouseDrag()
    {
        if (gameManagerController.editorMode || gameManagerController.currentRound == color)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(startPosition).z;
            Vector3 currentWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            currentWorldPos.y = startPosition.y;
            // 2. Calcula o vetor de deslocamento em relação ao ponto inicial
            Vector3 offset = currentWorldPos - startPosition;
            // 3. O CLAMP: Limita o tamanho desse vetor ao valor de 'maxDragDistance'
            limitedOffset = Vector3.ClampMagnitude(offset, maxDragDistance);

            UpdateArrow();
        }
    }

    void UpdateArrow()
    {
        if (arrowInstance == null) return;
        // Direção oposta ao drag (para onde o jogador vai ser lançado)
        Vector3 fireDirection = -limitedOffset;
        if (fireDirection.magnitude < 0.1f) return;

        // Posição: um pouco à frente do jogador na direção do lançamento
        arrowInstance.transform.position = startPosition + fireDirection.normalized * 1.2f;

        // Rotação apontando na direção do lançamento
        arrowInstance.transform.rotation = Quaternion.LookRotation(fireDirection, Vector3.up)
                                 * Quaternion.Euler(-90f, -90f, 0);

        // Escala proporcional à potência (0 a 1)
        float power = limitedOffset.magnitude / maxDragDistance;
        float arrowLength = Mathf.Lerp(0.5f, 2.5f, power);
        arrowInstance.transform.localScale = new Vector3(1f, 1f, arrowLength);


        // Cor pelo gradiente conforme a potência
        if (arrowGradient != null)
        {
            Renderer rend = arrowInstance.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = arrowGradient.Evaluate(power);
        }
    }

    void OnMouseUp()
    {
        if (gameManagerController.editorMode || gameManagerController.currentRound == color)
        {
            // 4. Move o objeto para a posição inicial + o deslocamento limitado
            //transform.position = startPosition + limitedOffset;
            isDragging = false;
            // Calcula a força baseada na distância de arrasto
            if (arrowInstance != null)
                arrowInstance.SetActive(false);

            Vector3 forceDirection = startPosition - (startPosition + limitedOffset);
            rb.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
            addJogada?.Invoke();
        }
    }

    void resetPosition()
    {
        transform.position = initialPosition; 
        rb.isKinematic = true;
        rb.isKinematic = false;

        if (arrowInstance != null)
            arrowInstance.SetActive(false);
    }

    void OnTriggerEnter(Collider other) // Se sair do mundo, volta pra posição inicial
    {
        if (other.gameObject.CompareTag("BorderLimit")) 
        {
            resetPosition();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Orange ou Green
        /**
        if (collision.gameObject.CompareTag("Ball") && gameManagerController.currentRound == color)
        {
            addJogada?.Invoke();
        }**/
    }
}
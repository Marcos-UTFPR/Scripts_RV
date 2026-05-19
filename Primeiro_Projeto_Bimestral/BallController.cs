using UnityEngine;
using System;

public class BallController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;
    public GameObject gameManager;
    private GameManagerController gameManagerController;    
    public static event Action orangeGoal;
    public static event Action greenGoal;
    public static event Action changePlayerTurn;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        gameManagerController = gameManager.GetComponent<GameManagerController>();
    }

    void resetPosition()
    {
        transform.position = initialPosition;
        rb.isKinematic = true; // Reseta todo o movimento
        rb.isKinematic = false;
    }

    void OnTriggerEnter(Collider other) // Se sair do mundo, volta pra posição inicial
    {
        if (other.gameObject.CompareTag("BorderLimit"))
        {
            Debug.Log("The ball has entered the BorderLimit trigger");
            resetPosition();
        } else {
            if (other.gameObject.CompareTag("OrangeGoal"))
            {
                greenGoal?.Invoke(); // Gol do time verde
                resetPosition();
            } else {
                if (other.gameObject.CompareTag("GreenGoal"))
                {
                    orangeGoal?.Invoke(); // Gol do time laranja
                    resetPosition();
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Orange ou Green
        if (collision.gameObject.CompareTag("GreenPlayer") && gameManagerController.currentRound != "Green")
        {
            changePlayerTurn?.Invoke();
        } else {
            if (collision.gameObject.CompareTag("OrangePlayer") && gameManagerController.currentRound != "Orange")
            {
                changePlayerTurn?.Invoke();
            }
        }
    }
}

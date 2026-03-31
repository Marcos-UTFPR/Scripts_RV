using UnityEngine;

public class CameraController : MonoBehaviour
{
        
    public GameObject player; // Ponteiro para pegar a posição do jogador
    private Vector3 distancia;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distancia = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = distancia + player.transform.position;
    }
}

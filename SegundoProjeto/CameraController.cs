using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Ponteiro para pegar a posi��o do jogador
    private Vector3 distancia;
    private bool isUpsideDown = false; // Flag da armadilha

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distancia = transform.position - player.transform.position;
    }

    void LateUpdate() // Movimento da Câmera
    {
        if (player != null)
        {
            transform.position = player.transform.position + (player.transform.rotation * distancia); 
            transform.LookAt(player.transform.position);
            if (isUpsideDown)
            {
                transform.Rotate(0, 0, 180);
            }
        }
    }
    
    private void OnEnable()
    {
        TrapCameraController.triggerTrap += Rotate;
    }

    private void OnDisable()
    {
        TrapCameraController.triggerTrap -= Rotate;
    }

    private void Rotate()
    {
        Debug.Log("Ativando a pior armadilha...");
        isUpsideDown = true;
    }
}
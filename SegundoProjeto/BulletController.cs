using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    public GameObject player; // Ponteiro para pegar a posi��o do jogador
    private PlayerController playerController;

    public static event Action hitEnemy;

    void Start()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) // Player não pode se matar com as próprias balas
        {
            Destroy(collision.gameObject);

            if (collision.gameObject.CompareTag("Enemy"))
            {
                hitEnemy?.Invoke();
                float randomX = UnityEngine.Random.Range(-15, 15); // O Enemy deveria ser instanciado por um GameManager, n�o pela Bullet
                float randomZ = UnityEngine.Random.Range(-15, 15);

                GameObject enemy = Instantiate(Resources.Load("Enemy", typeof(GameObject))) as GameObject;
                enemy.transform.position = new Vector3(randomX, 1, randomZ);
                enemy.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            }

            Destroy(gameObject); // A Bullet tem que ser a �ltima a ser deletada
        }
    }
}

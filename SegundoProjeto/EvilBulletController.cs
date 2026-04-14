using UnityEngine;
using System;
public class EvilBulletController : MonoBehaviour
{
    public static Action hitPlayer;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Player não pode se matar com as próprias balas
        {
            hitPlayer?.Invoke();
            Destroy(collision.gameObject);

            Destroy(gameObject); // A Bullet tem que ser a �ltima a ser deletada
        }
    }
}
